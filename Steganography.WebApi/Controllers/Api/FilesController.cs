using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Http;
using Steganography.Engine;
using Steganography.WebApi.Models;

namespace Steganography.WebApi.Controllers.Api
{
    [RoutePrefix("api/Files")]
    public class FilesController : ApiController
    {
        [Route("Upload")]
        public IHttpActionResult Upload(DataModel data)
        {
            try
            {
                var img = "";
                var image = DataImage.TryParse(data.ImageBase64);
                var replace = image.RawData;
                if (replace != null)
                {
                    var guid = Guid.NewGuid().ToString();
                    var extension = Utils.GetExtension(image);
                    img = guid + extension;
                }

                var password = ConfigurationManager.AppSettings["Key"] ?? "";
                string file1List;
                if (replace != null)
                {
                    var filePath = HttpContext.Current.Server.MapPath("~/" + img);
                    var filePathResult = HttpContext.Current.Server.MapPath("~/NewImage/" + img);

                    file1List = data.IsExtraction
                        ? Utils.Hide2(data, image.Image, filePath, password, filePathResult)
                        : Utils.Extract2(image.Image, password, filePathResult);

                    var fileInfo = new FileInfo(filePathResult);


                    return Ok(new
                    {
                        path = data.IsExtraction ? "~/NewImage/" + img : img, 
                        data = file1List,
                        base64 = data.IsExtraction ? Convert.ToBase64String(File.ReadAllBytes(filePath)) : ""
                    });
                }

                file1List = null;
                return BadRequest("File Not Found");
            }
            catch (FileNotFoundException ex)
            {
                return BadRequest("File Not Found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}