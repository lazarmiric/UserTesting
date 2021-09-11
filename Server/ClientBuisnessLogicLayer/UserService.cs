using Server.DataAccessLayer;
using Server.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.ClientBuisnessLogicLayer
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repository;
        public UserService(IUserRepository repository)
        {
            this.repository = repository;
        }
        public string Delete(User entity)
        {
            if(entity == null)
                throw new ArgumentNullException();

            if (entity.UserID == null)
                throw new Exception("UserID cannot be null!");

            var user = SelectById(entity.UserID);

            if (user == null)
                throw new Exception("User does not exist!");

            try
            {
                var message = repository.Delete(user);
                return message;
            }
            catch (Exception)
            {
                throw new Exception("Error while deleting user!");
            }
        }

        public string Insert(User entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            if (!Validate(entity, out string validateMessage))
                throw new Exception("Invalid: " + validateMessage);
            try
            {
                var message = repository.Insert(entity);
                return message;
            }
            catch (Exception)
            {
                throw new Exception("Error while inserting a user!");
            }
        }

        public IEnumerable<User> SelectAll()
        {
            try
            {
                return repository.SelectAll();
            }
            catch (Exception)
            {
                throw new Exception("Error wile selecting users!");
            }
        }

        public User SelectById(int? id)
        {
            try
            {
                if (id == null)
                    throw new Exception("UserID cannot be null!");
                var user = repository.SelectById(id);
                if (user == null)
                    throw new Exception("User does not exist!");
                return user;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public string Update(User entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            if (entity.UserID == null)
                throw new Exception("UserID cannot be null!");

            if (!Validate(entity, out string validateMessage))
                throw new Exception("Invalid: " + validateMessage);

            try
            {
                var user = SelectById(entity.UserID);
            }
            catch (Exception)
            {
                throw new Exception("User does not exist!");
            }

            try
            {
                var message = repository.Update(entity);
                return message;
            }
            catch (Exception)
            {
                throw new Exception("Error while updating user!");
            }
        }

        public bool Validate(User entity, out string message)
        {
            message = string.Empty;

            if (string.IsNullOrEmpty(entity.FirstName))
            {
                message = "Enter first name!";
                return false;
            }
            else
            {
                if (!char.IsUpper(entity.FirstName.ElementAt(0)))
                {
                    message = "First name must start with upper case!";
                    return false;
                }
            }

            if (string.IsNullOrEmpty(entity.LastName))
            {
                message = "Enter last name!";
                return false;
            }
            else
            {
                if (!char.IsUpper(entity.LastName.ElementAt(0)))
                {
                    message = "Last name must start with upper case!";
                    return false;
                }
            }

            if (string.IsNullOrEmpty(entity.SocialIdentityNumber))
            {
                message = "Enter social identity number!";
                return false;
            }
            else
            {
                if (entity.SocialIdentityNumber.Length != 13)
                {
                    message = "Social identity number must have 13 characters!";
                    return false;
                }
            }

            if (string.IsNullOrEmpty(entity.Email))
            {
                message = "Enter email!";
                return false;
            }
            else
            {
                if (!entity.Email.EndsWith(".rs") || !entity.Email.Contains("@"))
                {
                    message = "Email must contain @ with .rs domain!";
                    return false;
                }
            }

            if (entity.Height != null && entity.Height < 35)
            {
                message = "Minimum height is 35";
                return false;
            }

            if (entity.Weight != null && (entity.Weight > 250 || entity.Weight < 10))
            {
                message = "Weight must be in a range of 10 to 250";
                return false;
            }

            if (string.IsNullOrEmpty(entity.BirthDate.ToString()))
            {
                message = "Enter date of birth!";
                return false;
            }

            if (entity.PlaceId == null)
            {
                message = "Enter a place!";
                return false;
            }

            if (!string.IsNullOrEmpty(entity.PhoneNumber) && !entity.PhoneNumber.StartsWith("+381") && (entity.PhoneNumber.Length > 13 || entity.PhoneNumber.Length < 12))
            {
                message = "Enter a phone number in a format: +381********!";
                return false;
            }

            return true;
        }
    }
}
