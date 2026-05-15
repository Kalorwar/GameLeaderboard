using GameLeaderboard.Application.DTOs;
using GameLeaderboard.Application.Interfaces;
using GameLeaderboard.Domain.Entities;
using GameLeaderboard.Domain.Exceptions;
using GameLeaderboard.Domain.Interfaces;

namespace GameLeaderboard.Application.Services;

public class AuthService : IAuthService
{
    private readonly IJwtProvider _jwtProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository, IUnitOfWork unitOfWork, IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _jwtProvider = jwtProvider;
    }

    public async Task<AuthResponse> LoginOrRegisterAsync(AuthRequest request,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByDeviceIdAsync(request.DeviceId, cancellationToken);

        if (user == null)
        {
            var isUsernameTaken =
                await _userRepository.IsUsernameTakenAsync(request.Username, Guid.Empty, cancellationToken);
            if (isUsernameTaken)
            {
                throw new ValidationException($"Username '{request.Username}' is already taken by another player.");
            }

            user = new User
            {
                DeviceId = request.DeviceId,
                Username = request.Username
            };

            await _userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        else if (user.Username != request.Username)
        {
            var isUsernameTaken =
                await _userRepository.IsUsernameTakenAsync(request.Username, user.Id, cancellationToken);
            if (isUsernameTaken)
            {
                throw new ValidationException($"Username '{request.Username}' is already taken by another player.");
            }

            user.Username = request.Username;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        var token = _jwtProvider.GenerateToken(user);

        return new AuthResponse(token, user.Id, user.Username);
    }
}