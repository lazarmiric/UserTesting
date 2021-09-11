using Server.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Server.DataAccessLayer
{
    public class UserRepository : IUserRepository
    {
        Broker broker;
        SqlTransaction transaction;

        public UserRepository()
        {
            broker = Broker.BrokerInstance();
        }

        public string Delete(User entity)
        {
            transaction = null;
            try
            {
                broker.Connection.Open();
                transaction = broker.Connection.BeginTransaction();
                broker.Command =
                    new SqlCommand("", broker.Connection, transaction)
                    {
                        CommandText =
                    $"delete from dbo.[User] where UserID = {entity.UserID}"
                    };

                broker.Command.ExecuteNonQuery();
                Save();
                return $"User {entity.FirstName} {entity.LastName} is successfully deleted!";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return ex.Message.ToString();
            }
            finally
            {
                if (broker.Connection != null)
                    broker.Connection.Close();
            }
        }

        public string Insert(User entity)
        {
            transaction = null;
            string message = string.Empty;

            try
            {
                //entity.UserID = broker.GetId("SocialIdentityNumber", "User");
                broker.Connection.Open();

                broker.Command =
                    new SqlCommand("INSERT_dbo_User", broker.Connection);
                broker.Command.CommandType = CommandType.StoredProcedure;

              //  broker.Command.Parameters.AddWithValue("UserID", Convert.ToInt32(entity.UserID));
                broker.Command.Parameters.AddWithValue("SocialIdentityNumber", SqlDbType.VarChar).Value = entity.SocialIdentityNumber;
                broker.Command.Parameters.AddWithValue("FirstName", SqlDbType.NVarChar).Value = entity.FirstName;
                broker.Command.Parameters.AddWithValue("LastName", SqlDbType.NVarChar).Value = entity.LastName;
                if (entity.Height == 0)
                {
                    broker.Command.Parameters.AddWithValue("Height", DBNull.Value);
                }
                else
                {
                    broker.Command.Parameters.AddWithValue("Height", entity.Height);
                }

                if (entity.Weight == 0)
                {
                    broker.Command.Parameters.AddWithValue("Weight", DBNull.Value);
                }
                else
                {
                    broker.Command.Parameters.AddWithValue("Weight", entity.Weight);
                }
                if (String.IsNullOrEmpty(entity.EyeCollor))
                {
                    broker.Command.Parameters.AddWithValue("EyeColor", DBNull.Value);
                }
                else
                {                     
                    broker.Command.Parameters.AddWithValue("EyeColor", entity.EyeCollor);
                }
                if (String.IsNullOrEmpty(entity.PhoneNumber))
                {
                    broker.Command.Parameters.AddWithValue("PhoneNumber", DBNull.Value);
                }
                else
                {
                    broker.Command.Parameters.AddWithValue("PhoneNumber", entity.PhoneNumber);
                }
                broker.Command.Parameters.AddWithValue("Email", SqlDbType.VarChar).Value = entity.Email;
                broker.Command.Parameters.AddWithValue("BirthDate", SqlDbType.Date).Value = entity.BirthDate;
                if (String.IsNullOrEmpty(entity.Address))
                {
                    broker.Command.Parameters.AddWithValue("Address", DBNull.Value);
                }
                else
                {
                    broker.Command.Parameters.AddWithValue("Address", entity.Address);
                }
                if (String.IsNullOrEmpty(entity.Note))
                {
                    broker.Command.Parameters.AddWithValue("Note", DBNull.Value);
                }
                else
                {
                    broker.Command.Parameters.AddWithValue("Note", entity.Note);
                }
                broker.Command.Parameters.AddWithValue("PlaceID", entity.PlaceId);

                int result = broker.Command.ExecuteNonQuery();

                if (result == -1)
                {
                    message = $"{entity.FirstName} {entity.LastName} successfully inserted!";
                }
                else
                {
                    throw new Exception("There was error inserting a user!");
                }
                return message;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (broker.Connection != null)
                    broker.Connection.Close();
            }
        }

        public void Save()
        {
            transaction.Commit();
        }

        public IEnumerable<User> SelectAll()
        {
            try
            {
                List<User> people = new List<User>();

                broker.Connection.Open();
                broker.Command = new SqlCommand("", broker.Connection, transaction);
                broker.Command.CommandText = $"Select * from dbo.[User] u join Place place on (u.PlaceID = place.PlaceID)";

                SqlDataReader reader = broker.Command.ExecuteReader();

                while (reader.Read())
                {
                    User user = new User
                    {
                        UserID = reader.GetInt32(0),
                        SocialIdentityNumber= reader.GetString(1),
                        FirstName = reader.GetString(2),
                        LastName = reader.GetString(3),
                        Height = GetIntValueOrDefault(reader, "Height"),
                        Weight = GetIntValueOrDefault(reader, "Weight"),
                        EyeCollor = reader.GetString(6),                       
                        PhoneNumber = GetStringValueOrDefault(reader, "PhoneNumber"),
                        Email = reader.GetString(8),
                        BirthDate = reader.GetDateTime(9),
                        Address = GetStringValueOrDefault(reader, "Address"),
                        Note = GetStringValueOrDefault(reader, "Note"),
                        Place = new Place
                        {
                            PlaceId = reader.GetInt32(12),
                            CityName = reader.GetString(14),
                            Zipcode = reader.GetInt32(15),
                            Population = GetIntValueOrDefault(reader, "Population")
                        }

                    };
                    people.Add(user);
                }
                reader.Close();

                return people;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (broker.Connection != null)
                    broker.Connection.Close();
            }
        }

        public User SelectById(int? id)
        {
            User user = null;
            try
            {
                broker.Connection.Open();
                broker.Command = new SqlCommand("", broker.Connection, transaction);
                broker.Command.CommandText = $"Select * from dbo.[User] u join Place place on (u.PlaceID = place.PlaceID) where UserID = {id}";
                SqlDataReader reader = broker.Command.ExecuteReader();

                while (reader.Read())
                {
                    user = new User
                    {
                        UserID = reader.GetInt32(0),
                        SocialIdentityNumber = reader.GetString(1),
                        FirstName = reader.GetString(2),
                        LastName = reader.GetString(3),
                        Height = GetIntValueOrDefault(reader, "Height"),
                        Weight = GetIntValueOrDefault(reader, "Weight"),
                        EyeCollor = reader.GetString(6),
                        PhoneNumber = GetStringValueOrDefault(reader, "PhoneNumber"),
                        Email = reader.GetString(8),
                        BirthDate = reader.GetDateTime(9),
                        Address = GetStringValueOrDefault(reader, "Address"),
                        Note = GetStringValueOrDefault(reader, "Note"),
                        Place = new Place
                        {
                            PlaceId = reader.GetInt32(12),
                            CityName = reader.GetString(14),
                            Zipcode = reader.GetInt32(15),
                            Population = GetIntValueOrDefault(reader, "Population")
                        }
                    };
                }
                reader.Close();
                user.PlaceId = (int)user.Place.PlaceId;
                return user;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (broker.Connection != null)
                    broker.Connection.Close();
            }
        }

        public string Update(User entity)
        {
            transaction = null;

            try
            {
                broker.Connection.Open();
                transaction = broker.Connection.BeginTransaction();
                broker.Command =
                    new SqlCommand("", broker.Connection, transaction)
                    {
                        CommandText =
                    $"update dbo.[User] set SocialIdentityNumber ='{entity.SocialIdentityNumber}', FirstName = '{entity.FirstName}', LastName = '{entity.LastName}', BirthDate = '{entity.BirthDate}', Note = @Note, Address = @Address, Height = @Height, Weight = @Weight, EyeColor = @EyeColor, PhoneNumber = @PhoneNumber, Email = '{entity.Email}', PlaceId = {entity.PlaceId} where UserID = {entity.UserID}"
                    };
                if (String.IsNullOrEmpty(entity.Note))
                {
                    broker.Command.Parameters.AddWithValue("@Note", DBNull.Value);
                }
                else
                {
                    broker.Command.Parameters.AddWithValue("@Note", entity.Note);
                }
                if (String.IsNullOrEmpty(entity.Address))
                {
                    broker.Command.Parameters.AddWithValue("@Address", DBNull.Value);
                }
                else
                {
                    broker.Command.Parameters.AddWithValue("@Address", entity.Address);
                }
                if (entity.Height == 0)
                {
                    broker.Command.Parameters.AddWithValue("@Height", DBNull.Value);
                }
                else
                {
                    broker.Command.Parameters.AddWithValue("@Height", entity.Height);
                }
                if (entity.Weight == 0)
                {
                    broker.Command.Parameters.AddWithValue("@Weight", DBNull.Value);
                }
                else
                {
                    broker.Command.Parameters.AddWithValue("@Weight", entity.Weight);
                }
                if (String.IsNullOrEmpty(entity.EyeCollor))
                {
                    broker.Command.Parameters.AddWithValue("@EyeColor", DBNull.Value);
                }
                else
                {                   
                    broker.Command.Parameters.AddWithValue("@EyeColor", entity.EyeCollor);
                }
                if (String.IsNullOrEmpty(entity.PhoneNumber))
                {
                    broker.Command.Parameters.AddWithValue("@PhoneNumber", DBNull.Value);
                }
                else
                {
                    broker.Command.Parameters.AddWithValue("@PhoneNumber", entity.PhoneNumber);
                }

                broker.Command.ExecuteNonQuery();

                Save();

                return "Successful!";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return ex.Message.ToString();
            }
            finally
            {
                if (broker.Connection != null)
                    broker.Connection.Close();
            }
        }
        private string GetStringValueOrDefault(SqlDataReader reader, string column)
        {
            string data = (reader.IsDBNull(reader.GetOrdinal(column)))
                      ? "" : reader[column].ToString();
            return data;
        }

        private int GetIntValueOrDefault(SqlDataReader reader, string column)
        {
            int data = (reader.IsDBNull(reader.GetOrdinal(column)))
                        ? 0 : int.Parse(reader[column].ToString());
            return data;
        }
 

    }
}
