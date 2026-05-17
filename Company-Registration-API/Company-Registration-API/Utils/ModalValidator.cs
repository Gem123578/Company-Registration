using Company_Registration_API.Models;
using Company_Registration_API.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;

namespace Company_Registration_API.Utils
{
    public class ModalValidator
    {
        internal static void ValidateApplicantRegister(ApplicantRegisterDTO dto)
        {
            try
            {
                if(dto== null)
                {
                    throw new ApiException(CommonMessages.MSG_INVALID_VALUE);
                }

                if (string.IsNullOrEmpty(dto.FullName) || string.IsNullOrWhiteSpace(dto.FullName))
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_InvalidEnterValue, dto.FullName));
                }

                if (dto.FullName.Length > CommonConstants.MAX_FULLNAME_LENGHT)
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_INVALID_LENGTH, nameof(dto.FullName)));
                }

                if (string.IsNullOrEmpty(dto.EmailAddress) || string.IsNullOrWhiteSpace(dto.EmailAddress))
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_InvalidEnterValue, dto.EmailAddress));
                }

                if (dto.EmailAddress.Length > CommonConstants.MAX_EMAIL_LENGHT)
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_INVALID_LENGTH, nameof(dto.EmailAddress)));
                }

                if (!Regex.IsMatch(dto.EmailAddress, CommonConstants.EMAIL_PATTERN, RegexOptions.IgnoreCase))
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_InvalidEnterValue, nameof(dto.EmailAddress)));
                }

                if (string.IsNullOrEmpty(dto.Password) || string.IsNullOrWhiteSpace(dto.Password))
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_InvalidEnterValue, dto.Password));
                }

                if (dto.Password.Length < 6 || dto.Password.Length > 20)
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_INVALID_LENGTH, nameof(dto.Password)));
                }

                if (!Regex.IsMatch(dto.Password, CommonConstants.PASSWORD_PATTERN))
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_INVALID_PASS, nameof(dto.Password)));
                }

                if(string.IsNullOrEmpty(dto.PhoneNumber) || string.IsNullOrWhiteSpace(dto.PhoneNumber))
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_InvalidEnterValue, dto.PhoneNumber));
                }
                if (dto.PhoneNumber.Length > CommonConstants.MAX_Phno_LENGHT)
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_INVALID_LENGTH, nameof(dto.PhoneNumber)));
                }

                if (string.IsNullOrEmpty(dto.IdentityNumber) || string.IsNullOrWhiteSpace(dto.IdentityNumber))
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_InvalidEnterValue, dto.IdentityNumber));
                }
                if (dto.IdentityNumber.Length > CommonConstants.MAX_NRC_LENGHT)
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_INVALID_LENGTH, nameof(dto.IdentityNumber)));
                }

                if (string.IsNullOrEmpty(dto.Nationality) || string.IsNullOrWhiteSpace(dto.Nationality))
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_InvalidEnterValue, dto.Nationality));
                }

                if (dto.Nationality.Length > CommonConstants.MAX_NATIONALITY_LENGHT)
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_INVALID_LENGTH, nameof(dto.Nationality)));
                }
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static void ValidateCUSystemUser(long id, CreateUserDto dto)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ApiException(CommonMessages.MSG_INVALID_VALUE);
                }

                if (dto == null)
                {
                    throw new ApiException(CommonMessages.MSG_INVALID_VALUE);
                }

                if (string.IsNullOrEmpty(dto.UserName) || string.IsNullOrWhiteSpace(dto.UserName))
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_InvalidEnterValue, dto.UserName));
                }

                if (dto.UserName.Length > CommonConstants.MAX_FULLNAME_LENGHT)
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_INVALID_LENGTH, nameof(dto.UserName)));
                }

                if (string.IsNullOrEmpty(dto.EmailAddress) || string.IsNullOrWhiteSpace(dto.EmailAddress))
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_InvalidEnterValue, dto.EmailAddress));
                }

                if (dto.EmailAddress.Length > CommonConstants.MAX_EMAIL_LENGHT)
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_INVALID_LENGTH, nameof(dto.EmailAddress)));
                }

                if (!Regex.IsMatch(dto.EmailAddress, CommonConstants.EMAIL_PATTERN, RegexOptions.IgnoreCase))
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_InvalidEnterValue, nameof(dto.EmailAddress)));
                }

                if (string.IsNullOrEmpty(dto.Password) || string.IsNullOrWhiteSpace(dto.Password))
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_InvalidEnterValue, dto.Password));
                }

                if (dto.Password.Length < 6 || dto.Password.Length > 20)
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_INVALID_LENGTH, nameof(dto.Password)));
                }

                if (!Regex.IsMatch(dto.Password, CommonConstants.PASSWORD_PATTERN))
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_INVALID_PASS, nameof(dto.Password)));
                }

            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static void ValidateEmail(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email))
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_InvalidEnterValue, email));
                }

                if (email.Length > CommonConstants.MAX_EMAIL_LENGHT)
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_INVALID_LENGTH, nameof(email)));
                }

                if (!Regex.IsMatch(email, CommonConstants.EMAIL_PATTERN, RegexOptions.IgnoreCase))
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_InvalidEnterValue, nameof(email)));
                }
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        internal static void ValidateLoginUser(LoginDTO dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.EmailAddress) || string.IsNullOrWhiteSpace(dto.EmailAddress))
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_InvalidEnterValue, dto.EmailAddress));
                }

                if (dto.EmailAddress.Length > CommonConstants.MAX_EMAIL_LENGHT)
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_INVALID_LENGTH, nameof(dto.EmailAddress)));
                }

                if (!Regex.IsMatch(dto.EmailAddress, CommonConstants.EMAIL_PATTERN, RegexOptions.IgnoreCase))
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_InvalidEnterValue, nameof(dto.EmailAddress)));
                }

                if (string.IsNullOrEmpty(dto.Password) || string.IsNullOrWhiteSpace(dto.Password))
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_InvalidEnterValue, dto.Password));
                }

                if (dto.Password.Length < 6 || dto.Password.Length > 20)
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_INVALID_LENGTH, nameof(dto.Password)));
                }
                if (!Regex.IsMatch(dto.Password, CommonConstants.PASSWORD_PATTERN))
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_INVALID_PASS, nameof(dto.Password)));
                }
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static void ValidateSystemUserId(long id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ApiException(CommonMessages.MSG_INVALID_VALUE);
                }
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static void ValidateToken(string tokenString)
        {
            try
            {
                if (string.IsNullOrEmpty(tokenString) || string.IsNullOrWhiteSpace(tokenString))
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_InvalidEnterValue, tokenString));
                }

                if (tokenString.Length > CommonConstants.MAX_TOKEN_LENGHT)
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_INVALID_LENGTH, nameof(tokenString)));
                }

                if (!Regex.IsMatch(tokenString, CommonConstants.TOKEN_PATTERN))
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_InvalidEnterValue, nameof(tokenString)));
                }
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}