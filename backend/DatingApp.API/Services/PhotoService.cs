using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Helper;
using DatingApp.API.Interfaces;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Services
{
	public class PhotoService : IPhotoService
	{
		private readonly Cloudinary _cloudinary;

		public PhotoService(IOptions<CloudinaryHelper> config)
		{
			var acc = new Account
				(
					config.Value.CloudName,
					config.Value.ApiKey,
					config.Value.ApiSecret
				);
			_cloudinary = new Cloudinary(acc);
		}

		public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
		{
			var uploadResult = new ImageUploadResult();
			if (file.Length > 0)
			{
				using var stream = file.OpenReadStream();
				var uploadParams = new ImageUploadParams
				{
					File = new FileDescription(file.Name, stream),

					// 設定圖片長寬各500並裁成正方形 並以臉部為主要目標
					Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),

					// 指定資料夾
					Folder = "da-net7"
				};

				uploadResult = await _cloudinary.UploadAsync(uploadParams);
			}
			return uploadResult;
		}

		public async Task<DeletionResult> DeletePhotoAsync(string publicId)
		{
			var deleteParams = new DeletionParams(publicId);

			return await _cloudinary.DestroyAsync(deleteParams);
		}
	}
}