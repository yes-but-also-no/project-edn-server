using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using HttpMultipartParser;
using Swan.Logging;

namespace GameServer.Web
{
    /// <summary>
    /// This controller will accept images uploaded from the client
    /// Future use could allow for an account management page or something
    /// This requires that the clients localization.ini has an upload key pointing to this server /char
    /// For example: http://192.168.1.10:15180/chars
    /// </summary>
    public class CharacterController : WebApiController
    {
        [Route(HttpVerbs.Get, "/check_file.aspx")]
        public async Task<string> GetData([FormData] NameValueCollection data)
        {
            // Not sure what this does. Maybe it is used for not double uploading?
            return "Ok";
        }
        
        [Route(HttpVerbs.Post, "/upload_file.aspx")]
        public async Task PostData()
        {
            // Parse contents
            var parser = await MultipartFormDataParser.ParseAsync(Request.InputStream);

            // Log data
            parser.Parameters.ForEach(p => $"{p.Name}: ${p.Data}".Debug());

            // Verify there is files
            if (parser.Files.Count <= 0) return;
            
            // Create directory if needed
            if (!Directory.Exists("UserImages")) Directory.CreateDirectory("UserImages");
                
            parser.Files.ForEach(file =>
            {
                // Create each file
                var path = Path.Combine(Directory.GetCurrentDirectory(), "UserImages/", file.FileName);

                // Get streams
                using var output = File.OpenWrite(path);
                using var input = file.Data;

                // Read into memory
                var encryption = new List<byte>();
                var image = new List<byte>();
                
                // Start by finding jpeg header
                var readingImage = false;

                var read = input.ReadByte();

                // Check each step for it
                do
                {
                    // Get current byte
                    var curByte = (byte) read;

                    if (!readingImage)
                    {
                        // Peek the next byte
                        var next = input.ReadByte();

                        // Go back
                        input.Seek(-1, SeekOrigin.Current);
                        
                        // Found jpeg
                        if (curByte == 0xFF && next == 0xD8)
                        {
                            readingImage = true;
                        }
                    }

                    // Add to appropriate list
                    if (readingImage)
                        image.Add(curByte);
                    else
                        encryption.Add(curByte);

                    read = input.ReadByte();
                } while (read > -1);
                
                // Invert encryption bytes
                encryption.Reverse();

                // Convert to arrays
                var enc = encryption.ToArray();

                // Find spacing
                // I have yet to see one come in under 250, so we will start with this for now
                // Until I can figure out the actual logic to what decides the spacing
                var spacing = 250;

                // This is also hacky. I have noticed that sometimes the actual image has sequential bytes that mess this up.
                // So we will check and ignore if either the first or next byte seems to be in "order"
                while (image[spacing] != enc[0] || image[spacing - 1] == enc[0] - 1 || image[spacing + 1] == enc[0] + 1) spacing++;

                // remove the first one
                image.RemoveAt(spacing);

                // Remove encryption bytes
                var pos = 1;

                while (pos < enc.Length && (pos + 1) * spacing < image.Count)
                {
                    if (image[(pos + 1) * spacing] == enc[pos])
                        image.RemoveAt((pos + 1) * spacing);
                    else
                    {
                        $"Unable to find encryption byte at {(pos + 1) * spacing}! Expected {enc[pos]} but got {image[(pos + 1) * spacing]}!"
                            .Warn();
                        return;
                    }

                    pos++;
                }

                // Write to file
                output.Write(image.ToArray());
            });

            return;
        }
    }
}