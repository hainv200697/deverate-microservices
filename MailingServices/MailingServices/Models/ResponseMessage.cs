using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenServices.Models
{
    public class ResponseMessage
    {

        /// <summary>
        /// Initial
        /// </summary>
        public ResponseMessage()
        {
        }


        /// <summary>
        /// Trả về status code = 0 kèm message.
        /// </summary>
        /// <param name="message">TIn nhắn thông báo lỗi</param>
        /// <returns></returns>
        public ObjectResponse Error(string message)
        {
            ObjectResponse response = new ObjectResponse();
            response.status = new Status(0, message);
            return response;
        }

        public ObjectResponse Custom(int statusCode, string message)
        {
            ObjectResponse response = new ObjectResponse();
            response.status = new Status(statusCode, message);
            return response;
        }

        /// <summary>
        /// Trả về dữ liệu HttpResponseMessage kèm data
        /// </summary>
        /// <param name="data">Dữ liệu</param>
        /// <returns></returns>
        public ObjectResponse Success(dynamic data)
        {
            ObjectResponse response = new ObjectResponse
            {
                status = new Status(200, "Lấy dữ liệu thành công."),
                data = new Dictionary<string, dynamic>()
            };

            response.data.Add("data", data);
            return response;
        }

        /// <summary>
        /// Trả về dữ liệu HttpResponseMessage kèm lời nhắn
        /// </summary>
        /// <param name="message">Tin nhắn thông báo</param>
        /// <returns></returns>
        public ObjectResponse Success(string message)
        {
            ObjectResponse response = new ObjectResponse
            {
                status = new Status(200, message)
            };
            return response;
        }

        /// <summary>
        /// Trả về dữ liệu HttpResponseMessage kèm data và lời nhắn
        /// </summary>
        /// <param name="data">Dữ liệu</param>
        /// <param name="message">Tin nhắn thông báo</param>
        /// <returns></returns>
        public ObjectResponse Success(string message, dynamic data)
        {
            ObjectResponse response = new ObjectResponse
            {
                status = new Status(200, message),
                data = new Dictionary<string, dynamic>()
            };

            response.data.Add("data", data);
            return response;
        }
    }
}
