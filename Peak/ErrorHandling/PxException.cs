using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peak.Common.Enums;

namespace Peak.ErrorHandling
{

    /// <summary>
    /// 
    /// </summary>
    public class PxException : Exception
    {

        #region Private Member(s)

        private IPxErrorMessageProvider messageProvider;
        private string _message;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="priority"></param>
        /// <param name="messageProvider"></param>
        public PxException(string errorCode, ErrorPriority priority, IPxErrorMessageProvider messageProvider) : this(null, errorCode, priority, messageProvider)
        {

        }

        public PxException(string errorCode, ErrorPriority priority, string errorMessage)
        {
            ErrorCode = errorCode;
            this.Priority = priority;
            _message = errorMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="errorCode"></param>
        /// <param name="priority"></param>
        /// <param name="messageProvider"></param>
        public PxException(Exception innerException, string errorCode, ErrorPriority priority, IPxErrorMessageProvider messageProvider) : base(null, innerException)
        {
            this.messageProvider = messageProvider;
            ErrorCode = errorCode;
            if (innerException == null)
            {
                this.Priority = priority;
            }
            else if (innerException is PxException)
            {
                PxException ex = innerException as PxException;
                this.Priority = ex.Priority;
            }
            else
            {
                this.Priority = ErrorPriority.High;
            }
        }

        public PxException(Exception innerException, string errorCode, ErrorPriority priority, string errorMessage):base(errorMessage, innerException)
        {
            ErrorCode = errorCode;
            this.Priority = priority;
            _message = errorMessage;
        }


        #endregion

        #region Overriden Member(s)

        /// <summary>
        /// 
        /// </summary>
        public override string Message {
            get {
                if (!string.IsNullOrEmpty(_message))
                {
                    return _message;
                }
                if (messageProvider != null)
                {
                    return messageProvider.GetMessage();
                }
                return null;
            }
        }
        #endregion

        #region Property(s)
        /// <summary>
        /// 
        /// </summary>
        public string ErrorCode { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public ErrorPriority Priority { get; set; }

        /// <summary>
        /// Gerekli olduğu durumlarda hatayı oluştururken kullanılabilecek bir alan.
        /// </summary>
        public object Tag { get; set; }

        #endregion
    }
}
