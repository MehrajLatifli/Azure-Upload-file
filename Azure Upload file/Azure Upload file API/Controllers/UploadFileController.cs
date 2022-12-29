using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure_Upload_file_API.Services;
using Azure_Upload_file_Business.Abstract;
using Azure_Upload_file_DataAccess.Abstract;
using Azure_Upload_file_Entities.Concrete.DatabaseFirst;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;
using static System.Reflection.Metadata.BlobBuilder;

namespace Azure_Upload_file_API.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class UploadFileController : Controller
    {

        IInfoFileService  _infoFileService;
        public static IWebHostEnvironment _environment;

        public UploadFileController(IInfoFileService infoFileService, IWebHostEnvironment environment)
        {
            _infoFileService = infoFileService;
            _environment = environment;
        }


        [HttpGet("GetFile")]
        public IActionResult GetFile()
        {
            return Ok(_infoFileService.GetAll());
        }


        [HttpGet("GetFile/{id?}")]
        public IActionResult GetFile(int id)
        {
            var file = _infoFileService.GetById(id);

            try
            {
                if (file == null)
                {

                    return StatusCode(StatusCodes.Status404NotFound);
                }

                else
                {
                    return Ok(_infoFileService.GetAll().Where(f => f.IdFile == id));
                }
            }
            catch (Exception)
            {

            }
            return BadRequest();

        }

        [HttpGet("ViewFile")]
        public async Task<IActionResult> ViewFile(string filename)
        {


            try
            {
                string connecectionstring = "DefaultEndpointsProtocol=https;AccountName=myuploadfile;AccountKey=AjSdj+b8KIiNe9Tt6Uwi6153NzCTinWDorqBEi+YxluF5Rx6Vhw+0aXdVUQmt/3Vv0RGY1akLjri+AStW+u5UA==;EndpointSuffix=core.windows.net";


                string containerName = "uploadfilefromapi";

                var file = filename;
                var storageAccount = CloudStorageAccount.Parse(connecectionstring);
                var blobClient = storageAccount.CreateCloudBlobClient();

                CloudBlobContainer container = blobClient.GetContainerReference(containerName);
                CloudBlockBlob blob = container.GetBlockBlobReference(file);

                Stream blobStream = await blob.OpenReadAsync();

                return File(blobStream, blob.Properties.ContentType, file);

            }
            catch (Exception)
            {
                return StatusCode(404);

            }


        }


        [HttpPost("PostFile")]
        public async Task<string> PostFile([FromForm] UploadFile uploadFile)
        {

            try
            {
                if (Directory.Exists(_environment.WebRootPath + "\\Upload\\"))
                {
                    DirectoryInfo d = new DirectoryInfo(_environment.WebRootPath + "\\Upload\\"); //Assuming Test is your Folder

                    FileInfo[] Files = d.GetFiles(); //Getting Text files
                    string str = "";

                    foreach (FileInfo file in Files)
                    {
                        str = str + ", " + file.Name;

                        Debug.WriteLine(str);
                    }


                }

                if (uploadFile.files.Length > 0)
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\Upload\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\Upload\\");


                    }

                    using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + "\\Upload\\" + uploadFile.files.FileName))
                    {
                        uploadFile.files.CopyTo(fileStream);

                        fileStream.Flush();
                        fileStream.Close();
                    }


                    try
                    {





                        //return Ok("\\Upload\\" + uploadFile.files.FileName);

                        string connecectionstring = "DefaultEndpointsProtocol=https;AccountName=myuploadfile;AccountKey=AjSdj+b8KIiNe9Tt6Uwi6153NzCTinWDorqBEi+YxluF5Rx6Vhw+0aXdVUQmt/3Vv0RGY1akLjri+AStW+u5UA==;EndpointSuffix=core.windows.net";

                        string containerName = "uploadfilefromapi";
                        string folderpath = _environment.WebRootPath + "\\Upload\\";




          




                        var files = Directory.GetFiles(folderpath, "*", SearchOption.AllDirectories);

                        BlobContainerClient containerClient = new BlobContainerClient(connecectionstring, containerName);

                        foreach (var file in files)
                        {
                            var filePathOverCloud = file.Replace(folderpath, string.Empty);
                            using (MemoryStream memoryStream = new MemoryStream(System.IO.File.ReadAllBytes(file)))
                            {
                                try
                                {

                                    if (containerClient.GetBlobClient(file).ExistsAsync().Result == false)
                                    {
                                        containerClient.UploadBlob(filePathOverCloud, memoryStream);




                                    }


                                    else
                                    {

                                        Response.WriteAsJsonAsync("<script>alert('File alredy exist')</script>");
                                    }

                                }
                                catch (Exception)
                                {

                                }
                            }

                        }




                    }
                    catch (Exception)
                    {

                    }









                    Task.WaitAll();


                        Task.Run(async () =>
                        {



                            try
                            {

                                InfoFiles infoFile = new InfoFiles();

                                string connecectionstring = "DefaultEndpointsProtocol=https;AccountName=myuploadfile;AccountKey=AjSdj+b8KIiNe9Tt6Uwi6153NzCTinWDorqBEi+YxluF5Rx6Vhw+0aXdVUQmt/3Vv0RGY1akLjri+AStW+u5UA==;EndpointSuffix=core.windows.net";

                                string containerName = "uploadfilefromapi";
                                string folderpath = _environment.WebRootPath + "\\Upload\\";

                                // List all the blobs

                                var blobContainerClient = new BlobContainerClient(connecectionstring, containerName);

                                var blobs = blobContainerClient.GetBlobs();

                                foreach (var blob in blobs)
                                {
                                    Console.WriteLine(blob);





                                    infoFile.FileName = blob.Name;
                                    infoFile.FilePath = "https://myuploadfile.blob.core.windows.net/uploadfilefromapi/" + blob.Name;
                                    infoFile.FileSize = Math.Round((Convert.ToDouble(blob.Properties.ContentLength) * 0.00000095367432 / 1), 2).ToString() + "MB";


                                    if (!_infoFileService.GetAll().ToList().Exists(i => i.FileName == infoFile.FileName))
                                    {

                                        _infoFileService.Add(infoFile);

                                    }
                                }


                            }
                            catch (Exception ex)
                            {
                                // Log the exception
                            }











                });

 

                        return "Upload Done.";
                    

                }
            }
            catch (Exception ex)
            {

                return ex.Message.ToString();
            }

            return "";
        }




    }
}
