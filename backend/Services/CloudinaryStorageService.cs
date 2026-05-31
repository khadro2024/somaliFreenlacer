using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using SomaliFreelanceMarketplace.Configuration;

namespace SomaliFreelanceMarketplace.Services;

public class CloudinaryStorageService
{
    private readonly Cloudinary? _cloudinary;
    private readonly CloudinarySettings _settings;

    public CloudinaryStorageService(IOptions<CloudinarySettings> settings)
    {
        _settings = settings.Value;
        if (_settings.IsConfigured)
        {
            var account = new Account(_settings.CloudName, _settings.ApiKey, _settings.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }
    }

    public bool IsConfigured => _settings.IsConfigured && _cloudinary != null;

    public async Task<(string Url, string PublicId)> UploadImageAsync(Stream stream, string fileName, string subFolder)
    {
        if (_cloudinary == null)
            throw new InvalidOperationException("Cloudinary is not configured. Add Cloudinary settings to appsettings.json.");

        var folder = $"{_settings.Folder}/{subFolder}";
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, stream),
            Folder = folder,
            Overwrite = true,
            Transformation = new Transformation().Quality("auto").FetchFormat("auto")
        };

        var result = await _cloudinary.UploadAsync(uploadParams);
        if (result.Error != null)
            throw new InvalidOperationException(result.Error.Message);

        return (result.SecureUrl.ToString(), result.PublicId);
    }

    public async Task DeleteAsync(string publicId)
    {
        if (_cloudinary == null || string.IsNullOrEmpty(publicId)) return;
        await _cloudinary.DestroyAsync(new DeletionParams(publicId));
    }
}
