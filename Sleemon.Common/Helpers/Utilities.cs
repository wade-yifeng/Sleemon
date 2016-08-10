using System;
using System.IO;

namespace Sleemon.Common
{
    using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;

    public static class Utilities
    {
        public static XElement GetXElementFromObject<T>(T objectToSerialize)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            var doc = new XDocument();
            using (var xmlWriter = doc.CreateWriter())
            {
                xmlSerializer.Serialize(xmlWriter, objectToSerialize);
            }

            return doc.Root;
        }

        /// <summary>
        /// 验证是否指定的图片格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsImage(string str)
        {
            var isimage = false;
            var thestr = str.ToLower();
            //限定只能上传jpg和gif图片
            string[] allowExtension = { ".jpg", ".gif", ".bmp", ".png" };
            //对上传的文件的类型进行一个个匹对
            for (var i = 0; i < allowExtension.Length; i++)
            {
                if (thestr == allowExtension[i])
                {
                    isimage = true;
                    break;
                }
            }
            return isimage;
        }

        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Md5Hash(string input)
        {
            var md5Hasher = new MD5CryptoServiceProvider();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public static bool UpLoadImageFile(HttpPostedFileBase file, HttpServerUtilityBase server, out string message)
        {
            if (file == null)
            {
                message = "文件不能为空！";
                return false;
            }
            try
            {
                //取得文件的扩展名,并转换成小写
                var path = Path.GetExtension(file.FileName);
                if (path != null)
                {
                    string fileExtension = path.ToLower();
                    //验证上传文件是否图片格式
                    if (!IsImage(fileExtension))
                    {
                        message = "抱歉，只能上传图片！";
                        return false;
                    }
                    //对上传文件的大小进行检测，限定文件最大不超过2M
                    if (file.ContentLength > 2 * 1024 * 1024)
                    {
                        message = "文件大小不能超过2M,请重新选择！";
                        return false;
                    }
                    var filepath = "/Assets/upload/image/";
                    if (Directory.Exists(server.MapPath(filepath)) == false) //如果不存在就创建file文件夹
                    {
                        Directory.CreateDirectory(server.MapPath(filepath));
                    }
                    var virpath = filepath + Md5Hash(file.FileName) + fileExtension; //这是存到服务器上的虚拟路径
                    var mappath = server.MapPath(virpath); //转换成服务器上的物理路径
                    file.SaveAs(mappath);

                    message = virpath;

                    return true;
                }
                else
                {
                    message = "文件扩展名不能为空！";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }
    }
}
