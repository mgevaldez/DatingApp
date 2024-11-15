using CloudinaryDotNet.Actions;

namespace API.Interfaces;

public interface IPhotoService
{
    Task<ImageUploadResult> AddPhotoAsync(IFormFile photo);
    Task<DeletionResult> DeletePhotoAsync(string photoId);
}