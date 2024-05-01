using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CoffeeDBIntegrada.Core
{
    public class ConexionDB
    {
        private readonly SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CoffeeDBIntegrada.Properties.Settings.ProyectoDAMConnectionString"].ConnectionString);
        private DataTable TableDefault = new DataTable();
        private readonly string patron = "PatronContrasena";
        private const string Format = "{0:#.00}";


        #region Util
        public bool Repeat_DNI(string DNI)
        {
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT DNI FROM Usuario WHERE DNI = @DNI AND Eliminado = 0", con);
                cmd.Parameters.Add("@DNI", SqlDbType.VarChar).Value = DNI;
                string valid = (string)cmd.ExecuteScalar();
                con.Close();
                if (valid == DNI)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                con.Close();
                return false;
            }
        }

        public bool Repeat_CIF(string CIF)
        {
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT CIF FROM Proveedor WHERE CIF = @CIF AND Eliminado = 0", con);
                cmd.Parameters.Add("@CIF", SqlDbType.VarChar).Value = CIF;
                string valid = (string)cmd.ExecuteScalar();
                con.Close();
                if (valid == CIF)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                con.Close();
                return false;
            }
        }


        public bool Repeat_NombreProducto(string NombreProducto)
        {
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT NombreProducto FROM Producto WHERE NombreProducto = @NombreProducto AND Eliminado = 0", con);
                cmd.Parameters.Add("@NombreProducto", SqlDbType.VarChar).Value = NombreProducto;
                string valid = (string)cmd.ExecuteScalar();
                con.Close();
                if (valid == NombreProducto)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                con.Close();
                return false;
            }
        }

        public bool Repeat_Apodo(string Apodo)
        {
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT Apodo FROM Usuario WHERE Apodo = @Apodo AND Eliminado = 0", con);
                cmd.Parameters.Add("@Apodo", SqlDbType.VarChar).Value = Apodo;
                string valid = (string)cmd.ExecuteScalar();
                con.Close();
                if (valid == Apodo)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                con.Close();
                return false;
            }
        }



        public bool LowerThanOne(string DNI)
        {
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Usuario WHERE DNI = @DNI AND Eliminado = 0", con);
                cmd.Parameters.Add("@DNI", SqlDbType.VarChar).Value = DNI;
                int num = (int)cmd.ExecuteScalar();
                con.Close();
                if (num <= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                con.Close();
                return false;
            }
        }





        #endregion




        #region Usuarios

        #region Select
        public DataTable Select_Usuario_Full()
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdUsuario, Apodo, DNI, Privilegio, NombreUsuario, Apellido, FechaNac, Telefono, Email, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Usuario WHERE Eliminado = 0 ORDER BY IdUsuario ASC", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }

        public string[] Select_Usuario_Id(string IdUsuario)
        {
            string[] result = new string[9];
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT NombreUsuario, Apellido, DNI, Telefono, FORMAT(FechaNac, 'dd/MM/yyyy') as FechaNac, Privilegio, Email, Apodo FROM Usuario WHERE IdUsuario = @IdUsuario AND Eliminado = 0", con);
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dr.Read();
                result[0] = dr["NombreUsuario"].ToString();
                result[1] = dr["Apellido"].ToString();
                result[2] = dr["DNI"].ToString();
                result[3] = dr["Telefono"].ToString();
                result[4] = dr["FechaNac"].ToString();
                result[5] = dr["Privilegio"].ToString();
                result[6] = dr["Email"].ToString();
                result[7] = dr["Apodo"].ToString();

                dr.Close();

                con.Close();
                return result;
            }
            catch
            {
                con.Close();
                return result;
            }
        }



        public DataTable Usuario_Id(string IdUsuario)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdUsuario, Apodo, DNI, Privilegio, NombreUsuario, Apellido, Telefono, Email, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Usuario WHERE IdUsuario = @IdUsuario AND Eliminado = 0 ORDER BY IdUsuario DESC", con);
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Usuario_Full();
                return TableDefault;
            }


        }

        public DataTable Usuario_Apodo(string Apodo)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdUsuario, Apodo, DNI, Privilegio, NombreUsuario, Apellido, Telefono, Email, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Usuario WHERE Apodo LIKE @Apodo AND Eliminado = 0 ORDER BY Apodo ASC", con);
                cmd.Parameters.AddWithValue("@Apodo", "%" + Apodo + "%");
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Usuario_Full();
                return TableDefault;
            }
        }

        public DataTable Usuario_DNI(string DNI)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdUsuario, Apodo, DNI, Privilegio, NombreUsuario, Apellido, Telefono, Email, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Usuario WHERE DNI LIKE @DNI AND Eliminado = 0 ORDER BY DNI ASC", con);
                cmd.Parameters.AddWithValue("@DNI", "%" + DNI + "%");
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Usuario_Full();
                return TableDefault;
            }
        }

        public DataTable Usuario_Privilegio(string Privilegio)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdUsuario, Apodo, DNI, Privilegio, NombreUsuario, Apellido, Telefono, Email, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Usuario WHERE Privilegio = @Privilegio AND Eliminado = 0 ORDER BY IdUsuario DESC", con);
                cmd.Parameters.Add("@Privilegio", SqlDbType.VarChar).Value = Privilegio;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Usuario_Full();
                return TableDefault;
            }
        }

        public DataTable Usuario_Nombre(string NombreUsuario)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdUsuario, Apodo, DNI, Privilegio, NombreUsuario, Apellido, Telefono, Email, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Usuario WHERE NombreUsuario LIKE @NombreUsuario AND Eliminado = 0 ORDER BY NombreUsuario ASC", con);
                cmd.Parameters.AddWithValue("@NombreUsuario", "%" + NombreUsuario + "%");
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Usuario_Full();
                return TableDefault;
            }
        }

        public DataTable Usuario_Apellido(string Apellido)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdUsuario, Apodo, DNI, Privilegio, NombreUsuario, Apellido, Telefono, Email, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Usuario WHERE Apellido LIKE @Apellido AND Eliminado = 0 ORDER BY Apellido ASC", con);
                cmd.Parameters.AddWithValue("@Apellido", "%" + Apellido + "%");
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Usuario_Full();
                return TableDefault;
            }
        }

        public DataTable Usuario_FechaNac(string FechaNac)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdUsuario, Apodo, DNI, Privilegio, NombreUsuario, Apellido, FechaNac, Telefono, Email, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Usuario WHERE FechaNac = @FechaNac AND Eliminado = 0 ORDER BY FechaNac DESC", con);
                cmd.Parameters.Add("@FechaNac", SqlDbType.Date).Value = FechaNac;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Usuario_Full();
                return TableDefault;
            }
        }

        public DataTable Usuario_Telefono(string Telefono)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdUsuario, Apodo, DNI, Privilegio, NombreUsuario, Apellido, FechaNac, Telefono, Email, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Usuario WHERE Telefono = @Telefono AND Eliminado = 0 ORDER BY IdUsuario DESC", con);
                cmd.Parameters.Add("@Telefono", SqlDbType.Int).Value = Telefono;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Usuario_Full();
                return TableDefault;
            }
        }


        public DataTable Usuario_Email(string Email)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdUsuario, Apodo, DNI, Privilegio, NombreUsuario, Apellido, FechaNac, Telefono, Email, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Usuario WHERE Email LIKE @Email AND Eliminado = 0 ORDER BY Email ASC", con);
                cmd.Parameters.AddWithValue("@Email", "%" + Email + "%");
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Usuario_Full();
                return TableDefault;
            }
        }

        public DataTable Usuario_FechaModificacion(string FechaModificacion)
        {
            DateTime fecha2 = Convert.ToDateTime(FechaModificacion);
            string FechaConsulta2 = fecha2.AddDays(1).ToString("dd/MM/yyyy");

            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdUsuario, Apodo, DNI, Privilegio, NombreUsuario, Apellido, FechaNac, Telefono, Email, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Usuario WHERE FechaModificacion BETWEEN '" + FechaModificacion + "' AND '" + FechaConsulta2 + "' AND Eliminado = 0 ORDER BY FechaModificacion DESC", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }


        public DataTable Usuario_FechaCreacion(string FechaCreacion)
        {
            DateTime fecha2 = Convert.ToDateTime(FechaCreacion);
            string FechaConsulta2 = fecha2.AddDays(1).ToString("dd/MM/yyyy");

            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdUsuario, Apodo, DNI, Privilegio, NombreUsuario, Apellido, FechaNac, Telefono, Email, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Usuario WHERE FechaCreacion BETWEEN '" + FechaCreacion + "' AND '" + FechaConsulta2 + "' AND Eliminado = 0 ORDER BY FechaCreacion DESC", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }

        public int Get_Usuario_LogIn(string Apodo, string Contrasena)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdUsuario FROM Usuario WHERE Apodo = @Apodo AND CONVERT(VARCHAR(64), DECRYPTBYPASSPHRASE('PatronContrasena', Contrasena)) = @Contrasena AND Eliminado = 0", con);
                //(EncryptByPassPhrase('" + patron + "','" + Contrasena + "'))
                cmd.Parameters.Add("@Apodo", SqlDbType.VarChar).Value = Apodo;
                cmd.Parameters.Add("@Contrasena", SqlDbType.VarChar).Value = Contrasena;
                int IdUsuario = (int)cmd.ExecuteScalar();


                con.Close();

                return IdUsuario;
            }
            catch
            {
                con.Close();
                return 0;
            }
        }


        public string Get_Usuario_Apodo(string IdUsuario)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT Apodo FROM Usuario WHERE IdUsuario = @IdUsuario AND Eliminado = 0", con);
                cmd.Parameters.Add("@IdUsuario", SqlDbType.VarChar).Value = IdUsuario;
                string Apodo = (string)cmd.ExecuteScalar();

                con.Close();

                return Apodo;
            }
            catch
            {
                con.Close();
                return "No encontrado";
            }
        }

        public string Get_Usuario_Privilegio(string IdUsuario)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT Privilegio FROM Usuario WHERE IdUsuario = @IdUsuario AND Eliminado = 0", con);
                cmd.Parameters.Add("@IdUsuario", SqlDbType.VarChar).Value = IdUsuario;
                string Privilegio = (string)cmd.ExecuteScalar();

                con.Close();

                return Privilegio;
            }
            catch
            {
                con.Close();
                return "";
            }
        }


        public bool Get_Usuario_PrimerLog(int IdUsuario)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT PrimerLog FROM Usuario WHERE IdUsuario = @IdUsuario AND Eliminado = 0", con);
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;
                bool PrimerLog = (bool)cmd.ExecuteScalar();
                con.Close();

                return PrimerLog;
            }
            catch
            {
                con.Close();
                return false;
            }
        }


        public int Get_Usuario_Recordar()
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdUsuario FROM Usuario WHERE Recordar = 1 AND Eliminado = 0", con);
                int IdUsuario = (int)cmd.ExecuteScalar();

                con.Close();

                return IdUsuario;
            }
            catch
            {
                con.Close();
                return 0;
            }
        }



        public BitmapImage Usuario_PFP(string IdUsuario)
        {
            BitmapImage bi = new BitmapImage();
            try
            {
                con.Open();

                DataSet ds = new DataSet();
                SqlDataAdapter sqda = new SqlDataAdapter("SELECT PFP FROM Usuario WHERE IdUsuario = '" + IdUsuario + "' AND Eliminado = 0", con);
                sqda.Fill(ds);

                byte[] ImgProducto = (byte[])ds.Tables[0].Rows[0][0];
                MemoryStream stmr = new MemoryStream();
                stmr.Write(ImgProducto, 0, ImgProducto.Length);
                stmr.Position = 0;
                System.Drawing.Image drawing = System.Drawing.Image.FromStream(stmr);


                bi.BeginInit();
                MemoryStream ms = new MemoryStream();
                drawing.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                bi.StreamSource = ms;
                bi.EndInit();

                con.Close();
                return bi;
            }
            catch
            {
                con.Close();
                return bi;
            }
        }



        #endregion

        #region Insert

        public int Insert_Usuario(string DNI, string Apodo, string Contrasena, string NombreUsuario, string Apellido, string Telefono, string Email)
        {
            /*#region ImageDefault
            byte[] PFPDefault;

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(@"\..\..\CoffeeDBIntegrada\Images\Util\man.jpg", UriKind.Relative);
            bi.EndInit();

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bi));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                PFPDefault = ms.ToArray();
            }
            #endregion*/

            try
            {
                con.Open();


                SqlCommand com = new SqlCommand("INSERT INTO Usuario (DNI, Apodo, Contrasena, NombreUsuario, Apellido, Telefono, Email, FechaCreacion) OUTPUT INSERTED.IdUsuario VALUES " +
                                                "(@DNI, @Apodo, (EncryptByPassPhrase('" + patron + "', @Contrasena)), @NombreUsuario, @Apellido, @Telefono, @Email, @FechaCreacion)", con);
                com.Parameters.Add("@Contrasena", SqlDbType.VarChar).Value = Contrasena;
                com.Parameters.Add("@DNI", SqlDbType.VarChar).Value = DNI as string;
                com.Parameters.Add("@Apodo", SqlDbType.VarChar).Value = Apodo as string;
                com.Parameters.Add("@NombreUsuario", SqlDbType.VarChar).Value = NombreUsuario as string;
                com.Parameters.Add("@Apellido", SqlDbType.VarChar).Value = Apellido as string;
                if (Telefono == "") { com.Parameters.AddWithValue("@Telefono", DBNull.Value); } else { com.Parameters.Add("@Telefono", SqlDbType.Int).Value = Telefono; }
                com.Parameters.Add("@Email", SqlDbType.VarChar).Value = Email as string;
                //com.Parameters.AddWithValue("@PFP", SqlDbType.VarBinary).Value = PFPDefault;
                com.Parameters.AddWithValue("@FechaCreacion", SqlDbType.DateTime).Value = DateTime.Now;
                int IdUsuario = (int)com.ExecuteScalar();


                con.Close();
                return IdUsuario;
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
                return 0;
            }

        }



        public void Insert_Admin(string Apodo, string Contrasena)
        {
            try
            {
                con.Open();


                SqlCommand com = new SqlCommand("INSERT INTO Usuario (DNI, Apodo, Contrasena, NombreUsuario, Apellido, Telefono, Email, PrimerLog, Privilegio) OUTPUT INSERTED.IdUsuario VALUES " +
                                                "('12345678Z', @Apodo, (EncryptByPassPhrase('" + patron + "', @Contrasena)), 'administrador', 'global', 123456789, 'administrador.global@gmail.com',1,'Administrador')", con);
                com.Parameters.Add("@Contrasena", SqlDbType.VarChar).Value = Contrasena;
                com.Parameters.Add("@Apodo", SqlDbType.VarChar).Value = Apodo as string;
                com.ExecuteNonQuery();

                con.Close();

            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/
                MessageBox.Show("Error de conexión con la base de datos");
            }

        }


        #endregion

        #region Update
        public void Update_Usuario(string DNI, string Apodo, string Privilegio, string NombreUsuario, string Apellido, string Telefono, string FechaNac, string Email, string OldDNI)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdUsuario FROM Usuario WHERE DNI = @DNI", con);
                cmd.Parameters.Add("@DNI", SqlDbType.VarChar).Value = OldDNI;
                int IdUsuario = (int)cmd.ExecuteScalar();


                SqlCommand com = new SqlCommand("UPDATE Usuario SET DNI = @DNI, Apodo = @Apodo, Privilegio = @Privilegio, NombreUsuario = @NombreUsuario, Apellido = @Apellido, Telefono = @Telefono, FechaNac = @FechaNac, Email = @Email WHERE IdUsuario = @IdUsuario", con);
                com.Parameters.Add("@DNI", SqlDbType.VarChar).Value = DNI as string;
                com.Parameters.Add("@Apodo", SqlDbType.VarChar).Value = Apodo as string;
                com.Parameters.Add("@Privilegio", SqlDbType.VarChar).Value = Privilegio as string;
                com.Parameters.Add("@NombreUsuario", SqlDbType.VarChar).Value = NombreUsuario as string;
                com.Parameters.Add("@Apellido", SqlDbType.VarChar).Value = Apellido as string;
                if (Telefono == "") { com.Parameters.AddWithValue("@Telefono", DBNull.Value); } else { com.Parameters.Add("@Telefono", SqlDbType.Int).Value = Telefono; }
                if (FechaNac == "") { com.Parameters.AddWithValue("@FechaNac", DBNull.Value); } else { com.Parameters.Add("@FechaNac", SqlDbType.Date).Value = FechaNac; }
                com.Parameters.Add("@Email", SqlDbType.VarChar).Value = Email as string;
                com.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;
                com.ExecuteNonQuery();


                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }
        }

        public void Update_DNI_Usuario(string DNI, string IdUsuario)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Usuario SET DNI = @DNI, FechaModificacion = @FechaModificacion WHERE IdUsuario = @IdUsuario", con);
                com.Parameters.Add("@DNI", SqlDbType.VarChar).Value = DNI;
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }
        }



        public void Update_Apodo_Usuario(string Apodo, string IdUsuario)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Usuario SET Apodo = @Apodo, FechaModificacion = @FechaModificacion WHERE IdUsuario = @IdUsuario", con);
                com.Parameters.Add("@Apodo", SqlDbType.VarChar).Value = Apodo;
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }
        }


        public void Update_Privilegio_Usuario(string Privilegio, string IdUsuario)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Usuario SET Privilegio = @Privilegio, FechaModificacion = @FechaModificacion WHERE IdUsuario = @IdUsuario", con);
                com.Parameters.Add("@Privilegio", SqlDbType.VarChar).Value = Privilegio;
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }
        }


        public void Update_NombreUsuario_Usuario(string NombreUsuario, string IdUsuario)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Usuario SET NombreUsuario = @NombreUsuario, FechaModificacion = @FechaModificacion WHERE IdUsuario = @IdUsuario", con);
                com.Parameters.Add("@NombreUsuario", SqlDbType.VarChar).Value = NombreUsuario;
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }
        }


        public void Update_Apellido_Usuario(string Apellido, string IdUsuario)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Usuario SET Apellido = @Apellido, FechaModificacion = @FechaModificacion WHERE IdUsuario = @IdUsuario", con);
                com.Parameters.Add("@Apellido", SqlDbType.VarChar).Value = Apellido;
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }
        }

        public void Update_Telefono_Usuario(string Telefono, string IdUsuario)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Usuario SET Telefono = @Telefono, FechaModificacion = @FechaModificacion WHERE IdUsuario = @IdUsuario", con);
                com.Parameters.Add("@Telefono", SqlDbType.Int).Value = Telefono;
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }
        }

        public void Update_FechaNac_Usuario(string FechaNac, string IdUsuario)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Usuario SET FechaNac = '" + FechaNac + "' WHERE IdUsuario = @IdUsuario", con);
                com.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }
        }

        public void Update_Email_Usuario(string Email, string IdUsuario)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Usuario SET Email = @Email, FechaModificacion = @FechaModificacion WHERE IdUsuario = @IdUsuario", con);
                com.Parameters.Add("@Email", SqlDbType.VarChar).Value = Email;
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }
        }

        public void Update_Recordar_Usuario(int Recordar, string IdUsuario)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Usuario SET Recordar = @Recordar WHERE IdUsuario = @IdUsuario", con);
                com.Parameters.Add("@Recordar", SqlDbType.Int).Value = Recordar;
                com.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }
        }



        public void Update_PrimerLog_Usuario(int PrimerLog, string IdUsuario)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Usuario SET PrimerLog = @PrimerLog WHERE IdUsuario = @IdUsuario", con);
                com.Parameters.Add("@PrimerLog", SqlDbType.Int).Value = PrimerLog;
                com.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }
        }




        public void Update_Usuario_PFP(byte[] PFP, string IdUsuario)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Usuario SET PFP = @PFP WHERE IdUsuario = @IdUsuario", con);
                com.Parameters.Add("@PFP", SqlDbType.VarBinary).Value = PFP;
                com.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;
                com.ExecuteNonQuery();


                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }
        }

        public void Update_Usuario_Contrasena(string Contrasena, string IdUsuario)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Usuario SET Contrasena = (EncryptByPassPhrase('" + patron + "', @Contrasena)), PrimerLog = 0, FechaModificacion = @FechaModificacion WHERE IdUsuario = @IdUsuario", con);
                com.Parameters.Add("@Contrasena", SqlDbType.VarChar).Value = Contrasena;
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;
                com.ExecuteNonQuery();


                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }
        }



        public void Update_Usuario_Contrasena_SinPrimerLog(string Contrasena, string IdUsuario)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Usuario SET Contrasena = (EncryptByPassPhrase('" + patron + "', @Contrasena)), FechaModificacion = @FechaModificacion WHERE IdUsuario = @IdUsuario", con);
                com.Parameters.Add("@Contrasena", SqlDbType.VarChar).Value = Contrasena;
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;
                com.ExecuteNonQuery();


                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }
        }
        #endregion

        #region Delete
        public bool Delete_Usuario(string DNI)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("DELETE FROM Usuario WHERE DNI = @DNI", con);
                com.Parameters.Add("@DNI", SqlDbType.VarChar).Value = DNI;
                com.ExecuteNonQuery();

                con.Close();
                return true;
            }
            catch
            {
                con.Close();
                return false;
            }
        }

        /*public bool Delete_Usuario_Id(int IdUsuario)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("DELETE FROM Usuario WHERE IdUsuario = @IdUsuario", con);
                com.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;
                com.ExecuteNonQuery();

                con.Close();
                
            }
            catch
            {
                con.Close();
                return false;
            }
        }*/


        public bool Delete_Usuario_Id(int IdUsuario)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Usuario SET Eliminado = 1 WHERE IdUsuario = @IdUsuario", con);
                com.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;
                com.ExecuteNonQuery();

                con.Close();
                return true;
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
                return false;
            }
        }

        #endregion

        #endregion

        #region Productos

        #region Select
        public DataTable Select_Producto_Full()
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdProducto, NombreProducto, Clasificacion, ROUND(CAST (PrecioCosto AS decimal (30,2)),3) AS PrecioCosto, ROUND(CAST (PrecioBeneficio AS decimal (30,2)),3) AS PrecioBeneficio, ROUND(CAST (PrecioVenta AS decimal (30,2)),3) AS PrecioVenta, IVA, Stock, Categoria, Favorito, Ingredientes, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion  FROM Producto WHERE Eliminado = 0 ORDER BY IdProducto ASC", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();

                return dt;
            } catch
            {
                //MessageBox.Show(e.ToString());
                SqlCommand cmd1 = new SqlCommand("SELECT * FROM Producto WHERE Eliminado = 0 ORDER BY IdProducto ASC", con);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                da1.Fill(TableDefault);
                con.Close();
                return TableDefault;
            }
        }

        public string[] Select_Producto_Id(string IdProducto)
        {
            string[] result = new string[12];
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdProducto, Clasificacion, NombreProducto, ROUND(CAST (PrecioCosto AS decimal (30,2)),3) AS PrecioCosto, ROUND(CAST (PrecioBeneficio AS decimal (30,2)),3) AS PrecioBeneficio, ROUND(CAST (PrecioVenta AS decimal (30,2)),3) AS PrecioVenta, IVA, Stock, Categoria, Favorito, Ingredientes, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion  FROM Producto WHERE IdProducto = @IdProducto", con);
                cmd.Parameters.Add("@IdProducto", SqlDbType.Int).Value = IdProducto;
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dr.Read();
                result[0] = dr["IdProductoDB"].ToString();
                result[1] = dr["ClasificacionDB"].ToString();
                result[2] = dr["NombreProductoDB"].ToString();
                result[3] = dr["PrecioCostoDB"].ToString();
                result[4] = dr["PrecioBeneficioDB"].ToString();
                result[5] = dr["PrecioVentaDB"].ToString();
                result[6] = dr["IVADB"].ToString();
                result[7] = dr["StockDB"].ToString();
                result[8] = dr["CategoriaDB"].ToString();
                result[9] = dr["FavoritoDB"].ToString();
                //result[10] = dr["ImgDB"].ToString();
                result[11] = dr["IngredientesDB"].ToString();
                result[12] = dr["DescripcionDB"].ToString();


                dr.Close();
                con.Close();
                return result;
            }
            catch
            {
                con.Close();
                return result;
            }
        }

        public DataTable Producto_Categoria(string Categoria)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdProducto, Clasificacion, NombreProducto, ROUND(CAST (PrecioCosto AS decimal (30,2)),3) AS PrecioCosto, ROUND(CAST (PrecioBeneficio AS decimal (30,2)),3) AS PrecioBeneficio, ROUND(CAST (PrecioVenta AS decimal (30,2)),3) AS PrecioVenta, IVA, Stock, Categoria, Favorito, Ingredientes, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion  FROM Producto WHERE Categoria = @Categoria AND Eliminado = 0 ORDER BY IdProducto ASC", con);
                cmd.Parameters.Add("@Categoria", SqlDbType.VarChar).Value = Categoria as string;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                
                return TableDefault;
            }
        }

        public BitmapImage Producto_Img(string IdProducto)
        {
            BitmapImage bi = new BitmapImage();
            try
            {
                con.Open();

                DataSet ds = new DataSet();
                SqlDataAdapter sqda = new SqlDataAdapter("SELECT Img FROM Producto WHERE IdProducto = '" + IdProducto + "'", con);
                sqda.Fill(ds);

                byte[] ImgProducto = (byte[])ds.Tables[0].Rows[0][0];
                MemoryStream stmr = new MemoryStream();
                stmr.Write(ImgProducto, 0, ImgProducto.Length);
                stmr.Position = 0;
                System.Drawing.Image drawing = System.Drawing.Image.FromStream(stmr);


                bi.BeginInit();
                MemoryStream ms = new MemoryStream();
                drawing.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                bi.StreamSource = ms;
                bi.EndInit();

                con.Close();
                return bi;
            }
            catch
            {
                con.Close();
                return bi;
            }
        }


        public BitmapImage Producto_Img_NombreProducto(string NombreProducto)
        {
            BitmapImage bi = new BitmapImage();
            try
            {
                con.Open();

                DataSet ds = new DataSet();
                SqlDataAdapter sqda = new SqlDataAdapter("SELECT Img FROM Producto WHERE NombreProducto = '" + NombreProducto + "'", con);
                sqda.Fill(ds);

                byte[] ImgProducto = (byte[])ds.Tables[0].Rows[0][0];
                MemoryStream stmr = new MemoryStream();
                stmr.Write(ImgProducto, 0, ImgProducto.Length);
                stmr.Position = 0;
                System.Drawing.Image drawing = System.Drawing.Image.FromStream(stmr);


                bi.BeginInit();
                MemoryStream ms = new MemoryStream();
                drawing.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                bi.StreamSource = ms;
                bi.EndInit();

                con.Close();
                return bi;
            }
            catch
            {
                con.Close();
                return bi;
            }
        }


        public DataTable Producto_Id(string IdProducto)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdProducto, Clasificacion, NombreProducto, ROUND(CAST (PrecioCosto AS decimal (30,2)),3) AS PrecioCosto, ROUND(CAST (PrecioBeneficio AS decimal (30,2)),3) AS PrecioBeneficio, ROUND(CAST (PrecioVenta AS decimal (30,2)),3) AS PrecioVenta, IVA, Stock, Categoria, Favorito, Ingredientes, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion  FROM Producto WHERE IdProducto = @IdProducto AND Eliminado = 0  ORDER BY IdProducto DESC", con);
                cmd.Parameters.Add("@IdProducto", SqlDbType.Int).Value = IdProducto;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                
                return TableDefault;
            }


        }

        public DataTable Producto_Clasificacion(string Clasificacion)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdProducto, Clasificacion, NombreProducto, ROUND(CAST (PrecioCosto AS decimal (30,2)),3) AS PrecioCosto, ROUND(CAST (PrecioBeneficio AS decimal (30,2)),3) AS PrecioBeneficio, ROUND(CAST (PrecioVenta AS decimal (30,2)),3) AS PrecioVenta, IVA, Stock, Categoria, Favorito, Ingredientes, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion  FROM Producto WHERE Clasificacion = @Clasificacion AND Eliminado = 0  ORDER BY Clasificacion ASC", con);
                cmd.Parameters.Add("@Clasificacion", SqlDbType.VarChar).Value = Clasificacion as string;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                
                return TableDefault;
            }
        }


        public DataTable Producto_NombreProducto(string NombreProducto)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdProducto, Clasificacion, NombreProducto, ROUND(CAST (PrecioCosto AS decimal (30,2)),3) AS PrecioCosto, ROUND(CAST (PrecioBeneficio AS decimal (30,2)),3) AS PrecioBeneficio, ROUND(CAST (PrecioVenta AS decimal (30,2)),3) AS PrecioVenta, IVA, Stock, Categoria, Favorito, Ingredientes, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion  FROM Producto WHERE NombreProducto LIKE @NombreProducto AND Eliminado = 0 ORDER BY NombreProducto ASC", con);
                cmd.Parameters.AddWithValue("@NombreProducto", "%" + NombreProducto + "%");
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                
                return TableDefault;
            }
        }


        public DataTable Producto_PrecioCosto(string PrecioCosto)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdProducto, Clasificacion, NombreProducto, ROUND(CAST (PrecioCosto AS decimal (30,2)),3) AS PrecioCosto, ROUND(CAST (PrecioBeneficio AS decimal (30,2)),3) AS PrecioBeneficio, ROUND(CAST (PrecioVenta AS decimal (30,2)),3) AS PrecioVenta, IVA, Stock, Categoria, Favorito, Ingredientes, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion  FROM Producto WHERE PrecioCosto LIKE @PrecioCosto AND Eliminado = 0 ORDER BY PrecioCosto ASC", con);
                cmd.Parameters.AddWithValue("@PrecioCosto", "%" + PrecioCosto + "%");
                //cmd.Parameters.Add("@PrecioCosto", SqlDbType.Money).Value = PrecioCosto;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                
                return TableDefault;
            }
        }



        public DataTable Producto_PrecioBeneficio(string PrecioBeneficio)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdProducto, Clasificacion, NombreProducto, ROUND(CAST (PrecioCosto AS decimal (30,2)),3) AS PrecioCosto, ROUND(CAST (PrecioBeneficio AS decimal (30,2)),3) AS PrecioBeneficio, ROUND(CAST (PrecioVenta AS decimal (30,2)),3) AS PrecioVenta, IVA, Stock, Categoria, Favorito, Ingredientes, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion  FROM Producto WHERE PrecioBeneficio LIKE @PrecioBeneficio AND Eliminado = 0 ORDER BY PrecioBeneficio ASC", con);
                cmd.Parameters.AddWithValue("@PrecioBeneficio", "%" + PrecioBeneficio + "%");
                //cmd.Parameters.Add("@PrecioBeneficio", SqlDbType.Money).Value = PrecioBeneficio;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                
                return TableDefault;
            }
        }



        public DataTable Producto_PrecioVenta(string PrecioVenta)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdProducto, Clasificacion, NombreProducto, ROUND(CAST (PrecioCosto AS decimal (30,2)),3) AS PrecioCosto, ROUND(CAST (PrecioBeneficio AS decimal (30,2)),3) AS PrecioBeneficio, ROUND(CAST (PrecioVenta AS decimal (30,2)),3) AS PrecioVenta, IVA, Stock, Categoria, Favorito, Ingredientes, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion  FROM Producto WHERE PrecioVenta LIKE @PrecioVenta AND Eliminado = 0 ORDER BY PrecioVenta ASC", con);
                cmd.Parameters.AddWithValue("@PrecioVenta", "%" + PrecioVenta + "%");
                //cmd.Parameters.Add("@PrecioVenta", SqlDbType.Money).Value = PrecioVenta;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                
                return TableDefault;
            }
        }



        public DataTable Producto_IVA(string IVA)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdProducto, Clasificacion, NombreProducto, ROUND(CAST (PrecioCosto AS decimal (30,2)),3) AS PrecioCosto, ROUND(CAST (PrecioBeneficio AS decimal (30,2)),3) AS PrecioBeneficio, ROUND(CAST (PrecioVenta AS decimal (30,2)),3) AS PrecioVenta, IVA, Stock, Categoria, Favorito, Ingredientes, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion  FROM Producto WHERE IVA = @IVA AND Eliminado = 0 ORDER BY IVA ASC", con);
                cmd.Parameters.Add("@IVA", SqlDbType.Int).Value = IVA;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                
                return TableDefault;
            }
        }



        public DataTable Producto_Stock(string Stock)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdProducto, Clasificacion, NombreProducto, ROUND(CAST (PrecioCosto AS decimal (30,2)),3) AS PrecioCosto, ROUND(CAST (PrecioBeneficio AS decimal (30,2)),3) AS PrecioBeneficio, ROUND(CAST (PrecioVenta AS decimal (30,2)),3) AS PrecioVenta, IVA, Stock, Categoria, Favorito, Ingredientes, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion  FROM Producto WHERE Stock = @Stock AND Eliminado = 0 ORDER BY Stock ASC", con);
                cmd.Parameters.Add("@Stock", SqlDbType.Int).Value = Stock;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                
                return TableDefault;
            }
        }


        public DataTable Producto_Favorito(string Favorito)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdProducto, Clasificacion, NombreProducto, ROUND(CAST (PrecioCosto AS decimal (30,2)),3) AS PrecioCosto, ROUND(CAST (PrecioBeneficio AS decimal (30,2)),3) AS PrecioBeneficio, ROUND(CAST (PrecioVenta AS decimal (30,2)),3) AS PrecioVenta, IVA, Stock, Categoria, Favorito, Ingredientes, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion  FROM Producto WHERE Favorito = @Favorito AND Eliminado = 0 ORDER BY IdProducto ASC", con);
                cmd.Parameters.Add("@Favorito", SqlDbType.Int).Value = Favorito;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                
                return TableDefault;
            }
        }



        public DataTable Producto_Ingredientes(string Ingredientes)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdProducto, Clasificacion, NombreProducto, ROUND(CAST (PrecioCosto AS decimal (30,2)),3) AS PrecioCosto, ROUND(CAST (PrecioBeneficio AS decimal (30,2)),3) AS PrecioBeneficio, ROUND(CAST (PrecioVenta AS decimal (30,2)),3) AS PrecioVenta, IVA, Stock, Categoria, Favorito, Ingredientes, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion  FROM Producto WHERE Ingredientes LIKE @Ingredientes  AND Eliminado = 0 ORDER BY IdProducto DESC", con);
                cmd.Parameters.AddWithValue("@Ingredientes", "%" + Ingredientes + "%");
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                
                return TableDefault;
            }
        }



        public DataTable Producto_Descripcion(string Descripcion)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdProducto, Clasificacion, NombreProducto, ROUND(CAST (PrecioCosto AS decimal (30,2)),3) AS PrecioCosto, ROUND(CAST (PrecioBeneficio AS decimal (30,2)),3) AS PrecioBeneficio, ROUND(CAST (PrecioVenta AS decimal (30,2)),3) AS PrecioVenta, IVA, Stock, Categoria, Favorito, Ingredientes, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion  FROM Producto WHERE Descripcion LIKE @Descripcion AND Eliminado = 0 ORDER BY IdProducto DESC", con);
                cmd.Parameters.AddWithValue("@Descripcion", "%" + Descripcion + "%");
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                
                return TableDefault;
            }
        }


        public DataTable Producto_FechaCreacion(string FechaCreacion)
        {
            DateTime fecha2 = Convert.ToDateTime(FechaCreacion);
            string FechaConsulta2 = fecha2.AddDays(1).ToString("dd/MM/yyyy");

            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdProducto, Clasificacion, NombreProducto, ROUND(CAST (PrecioCosto AS decimal (30,2)),3) AS PrecioCosto, ROUND(CAST (PrecioBeneficio AS decimal (30,2)),3) AS PrecioBeneficio, ROUND(CAST (PrecioVenta AS decimal (30,2)),3) AS PrecioVenta, IVA, Stock, Categoria, Favorito, Ingredientes, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion  FROM Producto WHERE FechaCreacion BETWEEN '" + FechaCreacion + "' AND '" + FechaConsulta2 + "' AND Eliminado = 0 ORDER BY FechaCreacion DESC", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }


        public DataTable Producto_FechaMoficacion(string FechaMoficacion)
        {
            DateTime fecha2 = Convert.ToDateTime(FechaMoficacion);
            string FechaConsulta2 = fecha2.AddDays(1).ToString("dd/MM/yyyy");

            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdProducto, Clasificacion, NombreProducto, ROUND(CAST (PrecioCosto AS decimal (30,2)),3) AS PrecioCosto, ROUND(CAST (PrecioBeneficio AS decimal (30,2)),3) AS PrecioBeneficio, ROUND(CAST (PrecioVenta AS decimal (30,2)),3) AS PrecioVenta, IVA, Stock, Categoria, Favorito, Ingredientes, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion  FROM Producto WHERE FechaMoficacion BETWEEN '" + FechaMoficacion + "' AND '" + FechaConsulta2 + "' AND Eliminado = 0 ORDER BY FechaCreacion DESC", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }
        #endregion

        #region Insert

        public int Insert_Producto(string NombreProducto, string PrecioCosto, string PrecioBeneficio, string PrecioVenta)
        {
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand("INSERT INTO Producto (NombreProducto, PrecioCosto, PrecioBeneficio, PrecioVenta) OUTPUT INSERTED.IdProducto VALUES (@NombreProducto, @PrecioCosto, @PrecioBeneficio, @PrecioVenta)", con);
                com.Parameters.Add("@NombreProducto", SqlDbType.VarChar).Value = NombreProducto as string;
                if (PrecioCosto == "") { com.Parameters.Add("@PrecioCosto", SqlDbType.Money).Value = 0; } else { com.Parameters.Add("@PrecioCosto", SqlDbType.Money).Value = PrecioCosto; }
                if (PrecioBeneficio == "") { com.Parameters.Add("@PrecioBeneficio", SqlDbType.Money).Value = 0; } else { com.Parameters.Add("@PrecioBeneficio", SqlDbType.Money).Value = PrecioBeneficio; }
                if (PrecioVenta == "") { com.Parameters.Add("@PrecioVenta", SqlDbType.Money).Value = 0; } else { com.Parameters.Add("@PrecioVenta", SqlDbType.Money).Value = PrecioVenta; }
                int IdProducto = (int)com.ExecuteScalar();

                con.Close();

                return IdProducto;
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
                return 0;
            }

        }
        #endregion

        #region Update
        public void Update_Producto_NombreProducto(string NombreProducto, string IdProducto)
        {
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand("UPDATE Producto SET NombreProducto = @NombreProducto, FechaModificacion = @FechaModificacion WHERE IdProducto = @IdProducto", con);
                com.Parameters.Add("@NombreProducto", SqlDbType.VarChar).Value = NombreProducto as string;
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdProducto", SqlDbType.VarChar).Value = IdProducto;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }

        }


        public void Update_Producto_Precios(string PrecioCosto, string PrecioBeneficio, string PrecioVenta, string IdProducto)
        {
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand("UPDATE Producto SET PrecioCosto = @PrecioCosto, PrecioBeneficio = @PrecioBeneficio, PrecioVenta = @PrecioVenta, FechaModificacion = @FechaModificacion WHERE IdProducto = @IdProducto", con);
                com.Parameters.Add("@PrecioCosto", SqlDbType.Money).Value = PrecioCosto;
                com.Parameters.Add("@PrecioBeneficio", SqlDbType.Money).Value = PrecioBeneficio;
                com.Parameters.Add("@PrecioVenta", SqlDbType.Money).Value = PrecioVenta;
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdProducto", SqlDbType.VarChar).Value = IdProducto;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }

        }


        public void Update_Producto_Clasificacion(string Clasificacion, string IdProducto)
        {
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand("UPDATE Producto SET Clasificacion = @Clasificacion WHERE IdProducto = @IdProducto", con);
                com.Parameters.Add("@Clasificacion", SqlDbType.VarChar).Value = Clasificacion as string;
                com.Parameters.Add("@IdProducto", SqlDbType.VarChar).Value = IdProducto;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }

        }

        public void Update_Producto_IVA(string IVA, string IdProducto)
        {
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand("UPDATE Producto SET IVA = @IVA, FechaModificacion = @FechaModificacion WHERE IdProducto = @IdProducto", con);
                com.Parameters.Add("@IVA", SqlDbType.Int).Value = IVA;
                com.Parameters.Add("@IdProducto", SqlDbType.VarChar).Value = IdProducto;
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }

        }

        public void Update_Producto_Stock(string Stock, string IdProducto)
        {
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand("UPDATE Producto SET Stock = @Stock, FechaModificacion = @FechaModificacion WHERE IdProducto = @IdProducto", con);
                com.Parameters.Add("@Stock", SqlDbType.Int).Value = Stock;
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdProducto", SqlDbType.VarChar).Value = IdProducto;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }

        }


        public void Update_Producto_Stock_NombreProducto(string Stock, string NombreProducto)
        {
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand("UPDATE Producto SET Stock = @Stock WHERE NombreProducto = @NombreProducto", con);
                com.Parameters.Add("@Stock", SqlDbType.Int).Value = Stock;
                com.Parameters.Add("@NombreProducto", SqlDbType.VarChar).Value = NombreProducto;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }

        }


        public void Update_Producto_Categoria(string Categoria, string IdProducto)
        {
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand("UPDATE Producto SET Categoria = @Categoria, FechaModificacion = @FechaModificacion WHERE IdProducto = @IdProducto", con);
                com.Parameters.Add("@Categoria", SqlDbType.VarChar).Value = Categoria as string;
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdProducto", SqlDbType.VarChar).Value = IdProducto;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }

        }

        public void Update_Producto_Favorito(string Favorito, string IdProducto)
        {
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand("UPDATE Producto SET Favorito = @Favorito WHERE IdProducto = @IdProducto", con);
                com.Parameters.Add("@Favorito", SqlDbType.Int).Value = Favorito;
                com.Parameters.Add("@IdProducto", SqlDbType.VarChar).Value = IdProducto;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }

        }

        public void Update_Producto_Img(byte[] Img, string IdProducto)
        {
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand("UPDATE Producto SET Img = @Img, FechaModificacion = @FechaModificacion WHERE IdProducto = @IdProducto", con);
                if (Img == null) { com.Parameters.AddWithValue("@Img", DBNull.Value); } else { com.Parameters.Add("@Img", SqlDbType.VarBinary).Value = Img; }
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdProducto", SqlDbType.VarChar).Value = IdProducto;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }

        }

        public void Update_Producto_Ingredientes(string Ingredientes, string IdProducto)
        {
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand("UPDATE Producto SET Ingredientes = @Ingredientes, FechaModificacion = @FechaModificacion WHERE IdProducto = @IdProducto", con);
                com.Parameters.Add("@Ingredientes", SqlDbType.VarChar).Value = Ingredientes as string;
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdProducto", SqlDbType.VarChar).Value = IdProducto;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }

        }

        public void Update_Producto_Descripcion(string Descripcion, string IdProducto)
        {
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand("UPDATE Producto SET Descripcion = @Descripcion, FechaModificacion = @FechaModificacion WHERE IdProducto = @IdProducto", con);
                com.Parameters.Add("@Descripcion", SqlDbType.VarChar).Value = Descripcion as string;
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdProducto", SqlDbType.VarChar).Value = IdProducto;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }

        }



        public void Update_Producto_Terminado(string Ingredientes, string IdProducto)
        {
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand("UPDATE Producto SET Ingredientes = @Ingredientes, FechaModificacion = @FechaModificacion WHERE IdProducto = @IdProducto", con);
                com.Parameters.Add("@Ingredientes", SqlDbType.VarChar).Value = Ingredientes as string;
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdProducto", SqlDbType.VarChar).Value = IdProducto;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }

        }
        #endregion

        #region Delete

        /*
        public bool Delete_Producto_Id(string IdProducto)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("DELETE FROM Producto WHERE IdProducto = @IdProducto", con);
                com.Parameters.Add("@IdProducto", SqlDbType.VarChar).Value = IdProducto;
                com.ExecuteNonQuery();

                con.Close();
                return true;
            }
            catch
            {
                con.Close();
                return false;
            }
        }*/

        public bool Delete_Producto_Id(string IdProducto)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Producto SET Eliminado = 1 WHERE IdProducto = @IdProducto", con);
                com.Parameters.Add("@IdProducto", SqlDbType.VarChar).Value = IdProducto;
                com.ExecuteNonQuery();

                con.Close();
                return true;
            }
            catch
            {
                con.Close();
                return false;
            }
        }

        #endregion

        #endregion

        #region Proveedores

        #region Select
        public DataTable Select_Proveedor_Full()
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdProveedor, CIF, NombreProveedor, Telefono, Telefono2, Email, Poblacion, CodPostal, Direccion, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Proveedor WHERE Eliminado = 0 ORDER BY IdProveedor DESC", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }

        public string[] Select_Proveedor_Id(string IdProveedor)
        {
            string[] result = new string[9];
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdProveedor, CIF, NombreProveedor, Telefono, Telefono2, Email, Poblacion, CodPostal, Direccion, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Proveedor WHERE IdProveedor = @IdProveedor AND Eliminado = 0", con);
                cmd.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = IdProveedor;
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dr.Read();
                result[0] = dr["CIF"].ToString();
                result[1] = dr["NombreProveedor"].ToString();
                result[2] = dr["Telefono"].ToString();
                result[3] = dr["Telefono2"].ToString();
                result[4] = dr["Email"].ToString();
                result[5] = dr["Poblacion"].ToString();
                result[6] = dr["CodPostal"].ToString();
                result[7] = dr["Direccion"].ToString();
                result[8] = dr["Descripcion"].ToString();

                dr.Close();

                con.Close();
                return result;
            }
            catch
            {
                con.Close();
                return result;
            }
        }



        public DataTable Proveedor_Id(string IdProveedor)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdProveedor, CIF, NombreProveedor, Telefono, Telefono2, Email, Poblacion, CodPostal, Direccion, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Proveedor WHERE IdProveedor = @IdProveedor AND Eliminado = 0 ORDER BY IdProveedor DESC", con);
                cmd.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = IdProveedor;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Proveedor_Full();
                return TableDefault;
            }


        }

        public DataTable Proveedor_Nombre(string NombreProveedor)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdProveedor, CIF, NombreProveedor, Telefono, Telefono2, Email, Poblacion, CodPostal, Direccion, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Proveedor WHERE NombreProveedor LIKE @NombreProveedor AND Eliminado = 0 ORDER BY NombreProveedor ASC", con);
                cmd.Parameters.AddWithValue("@NombreProveedor", "%" + NombreProveedor + "%");
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Proveedor_Full();
                return TableDefault;
            }
        }

        public DataTable Proveedor_CIF(string CIF)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdProveedor, CIF, NombreProveedor, Telefono, Telefono2, Email, Poblacion, CodPostal, Direccion, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Proveedor WHERE CIF = @CIF AND Eliminado = 0 ORDER BY CIF ASC", con);
                cmd.Parameters.Add("@CIF", SqlDbType.VarChar).Value = CIF;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Proveedor_Full();
                return TableDefault;
            }
        }

        public DataTable Proveedor_Telefono(string Telefono)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdProveedor, CIF, NombreProveedor, Telefono, Telefono2, Email, Poblacion, CodPostal, Direccion, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Proveedor WHERE Telefono = @Telefono AND Eliminado = 0 ORDER BY IdProveedor DESC", con);
                cmd.Parameters.Add("@Telefono", SqlDbType.Int).Value = Telefono;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Proveedor_Full();
                return TableDefault;
            }
        }

        public DataTable Proveedor_Telefono2(string Telefono2)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdProveedor, CIF, NombreProveedor, Telefono, Telefono2, Email, Poblacion, CodPostal, Direccion, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Proveedor WHERE Telefono2 = @Telefono2 AND Eliminado = 0 ORDER BY IdProveedor DESC", con);
                cmd.Parameters.Add("@Telefono2", SqlDbType.Int).Value = Telefono2;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Proveedor_Full();
                return TableDefault;
            }
        }

        public DataTable Proveedor_Email(string Email)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdProveedor, CIF, NombreProveedor, Telefono, Telefono2, Email, Poblacion, CodPostal, Direccion, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Proveedor WHERE Email LIKE @Email AND Eliminado = 0 ORDER BY Email ASC", con);
                cmd.Parameters.AddWithValue("@Email", "%" + Email + "%");
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Proveedor_Full();
                return TableDefault;
            }
        }

        public DataTable Proveedor_Poblacion(string Poblacion)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdProveedor, CIF, NombreProveedor, Telefono, Telefono2, Email, Poblacion, CodPostal, Direccion, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Proveedor WHERE Poblacion = @Poblacion AND Eliminado = 0 ORDER BY Poblacion ASC", con);
                cmd.Parameters.Add("@Poblacion", SqlDbType.VarChar).Value = Poblacion;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Proveedor_Full();
                return TableDefault;
            }
        }

        public DataTable Proveedor_CodPostal(string CodPostal)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdProveedor, CIF, NombreProveedor, Telefono, Telefono2, Email, Poblacion, CodPostal, Direccion, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Proveedor WHERE CodPostal = @CodPostal AND Eliminado = 0 ORDER BY CodPostal ASC", con);
                cmd.Parameters.Add("@CodPostal", SqlDbType.VarChar).Value = CodPostal;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Proveedor_Full();
                return TableDefault;
            }
        }

        public DataTable Proveedor_Direccion(string Direccion)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdProveedor, CIF, NombreProveedor, Telefono, Telefono2, Email, Poblacion, CodPostal, Direccion, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Proveedor WHERE Direccion LIKE @Direccion AND Eliminado = 0 ORDER BY Direccion ASC", con);
                cmd.Parameters.AddWithValue("@Direccion", "%" + Direccion + "%");
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Proveedor_Full();
                return TableDefault;
            }
        }

        public DataTable Proveedor_FechaModificacion(string FechaModificacion)
        {
            DateTime fecha2 = Convert.ToDateTime(FechaModificacion);
            string FechaConsulta2 = fecha2.AddDays(1).ToString("dd/MM/yyyy");

            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdProveedor, CIF, NombreProveedor, Telefono, Telefono2, Email, Poblacion, CodPostal, Direccion, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Proveedor WHERE FechaModificacion BETWEEN '" + FechaModificacion + "' AND '" + FechaConsulta2 + "' AND Eliminado = 0 ORDER BY FechaModificacion DESC", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }



        public DataTable Proveedor_FechaCreacion(string FechaCreacion)
        {
            DateTime fecha2 = Convert.ToDateTime(FechaCreacion);
            string FechaConsulta2 = fecha2.AddDays(1).ToString("dd/MM/yyyy");

            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdProveedor, CIF, NombreProveedor, Telefono, Telefono2, Email, Poblacion, CodPostal, Direccion, Descripcion, FORMAT(FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Proveedor WHERE FechaCreacion BETWEEN '" + FechaCreacion + "' AND '" + FechaConsulta2 + "' AND Eliminado = 0 ORDER BY FechaCreacion DESC", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }

        #endregion

        #region Insert

        public int Insert_Proveedor(string CIF, string NombreProveedor, string Telefono, string Email, string Poblacion, string CodPostal)
        {

            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("INSERT INTO Proveedor (CIF, NombreProveedor, Telefono, Email, Poblacion, CodPostal) OUTPUT INSERTED.IdProveedor VALUES (@CIF, @NombreProveedor, @Telefono, @Email, @Poblacion, @CodPostal)", con);
                com.Parameters.Add("@CIF", SqlDbType.VarChar).Value = CIF as string;
                com.Parameters.Add("@NombreProveedor", SqlDbType.VarChar).Value = NombreProveedor as string;
                com.Parameters.Add("@Telefono", SqlDbType.Int).Value = Telefono;
                com.Parameters.Add("@Email", SqlDbType.VarChar).Value = Email as string;
                if (string.IsNullOrEmpty(Poblacion)) { com.Parameters.AddWithValue("@Poblacion", DBNull.Value); } else { com.Parameters.Add("@Poblacion", SqlDbType.VarChar).Value = Poblacion; }
                com.Parameters.Add("@CodPostal", SqlDbType.VarChar).Value = CodPostal;
                int IdProveedor = (int)com.ExecuteScalar();

                con.Close();

                return IdProveedor;
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
                return 0;
            }

        }

        #endregion

        #region Update

        public void Update_CIF_Proveedor(string CIF, string IdProveedor)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Proveedor SET CIF = @CIF, FechaModificacion = @FechaModificacion WHERE IdProveedor = @IdProveedor", con);
                com.Parameters.Add("@CIF", SqlDbType.VarChar).Value = CIF as string;
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = IdProveedor;
                com.ExecuteNonQuery();


                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }
        }

        public void Update_NombreProveedor_Proveedor(string NombreProveedor, string IdProveedor)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Proveedor SET NombreProveedor = @NombreProveedor, FechaModificacion = @FechaModificacion WHERE IdProveedor = @IdProveedor", con);
                com.Parameters.Add("@NombreProveedor", SqlDbType.VarChar).Value = NombreProveedor as string;
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = IdProveedor;
                com.ExecuteNonQuery();


                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }
        }

        public void Update_Telefono_Proveedor(string Telefono, string IdProveedor)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Proveedor SET Telefono = @Telefono, FechaModificacion = @FechaModificacion WHERE IdProveedor = @IdProveedor", con);
                com.Parameters.Add("@Telefono", SqlDbType.Int).Value = int.Parse(Telefono);
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = IdProveedor;
                com.ExecuteNonQuery();


                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show(ex.ToString()+"Error de conexión con la base de datos");
            }
        }


        public void Update_Telefono2_Proveedor(string Telefono2, string IdProveedor)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Proveedor SET Telefono2 = @Telefono2, FechaModificacion = @FechaModificacion WHERE IdProveedor = @IdProveedor", con);
                if (Telefono2 == "") { com.Parameters.AddWithValue("@Telefono2", DBNull.Value); } else { com.Parameters.Add("@Telefono2", SqlDbType.Int).Value = Telefono2; }
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = IdProveedor;
                com.ExecuteNonQuery();


                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }
        }


        public void Update_Email_Proveedor(string Email, string IdProveedor)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Proveedor SET Email = @Email, FechaModificacion = @FechaModificacion WHERE IdProveedor = @IdProveedor", con);
                com.Parameters.Add("@Email", SqlDbType.VarChar).Value = Email as string;
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = IdProveedor;
                com.ExecuteNonQuery();


                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }
        }


        public void Update_Poblacion_Proveedor(string Poblacion, string IdProveedor)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Proveedor SET Poblacion = @Poblacion, FechaModificacion = @FechaModificacion WHERE IdProveedor = @IdProveedor", con);
                com.Parameters.Add("@Poblacion", SqlDbType.VarChar).Value = Poblacion as string;
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = IdProveedor;
                com.ExecuteNonQuery();


                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }
        }



        public void Update_CodPostal_Proveedor(string CodPostal, string IdProveedor)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Proveedor SET CodPostal = @CodPostal, FechaModificacion = @FechaModificacion WHERE IdProveedor = @IdProveedor", con);
                com.Parameters.Add("@CodPostal", SqlDbType.Int).Value = CodPostal;
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = IdProveedor;
                com.ExecuteNonQuery();


                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }
        }


        public void Update_Direccion_Proveedor(string Direccion, string IdProveedor)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Proveedor SET Direccion = @Direccion WHERE IdProveedor = @IdProveedor", con);
                com.Parameters.Add("@Direccion", SqlDbType.VarChar).Value = Direccion as string;
                com.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = IdProveedor;
                com.ExecuteNonQuery();


                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }
        }



        public void Update_Descripcion_Proveedor(string Descripcion, string IdProveedor)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Proveedor SET Descripcion = @Descripcion, FechaModificacion = @FechaModificacion WHERE IdProveedor = @IdProveedor", con);
                com.Parameters.Add("@Descripcion", SqlDbType.VarChar).Value = Descripcion as string;
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = IdProveedor;
                com.ExecuteNonQuery();


                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }
        }
        #endregion

        #region Delete
        public bool Delete_Proveedor(string CIF)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("DELETE FROM Proveedor WHERE CIF = @CIF", con);
                com.Parameters.Add("@CIF", SqlDbType.VarChar).Value = CIF;
                com.ExecuteNonQuery();

                con.Close();
                return true;
            }
            catch
            {
                con.Close();
                return false;
            }
        }

        /*public bool Delete_Proveedor_Id(string IdProveedor)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("DELETE FROM Proveedor WHERE IdProveedor = @IdProveedor", con);
                com.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = IdProveedor;
                com.ExecuteNonQuery();

                con.Close();
                return true;
            }
            catch
            {
                con.Close();
                return false;
            }
        }*/

        public bool Delete_Proveedor_Id(string IdProveedor)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Proveedor SET Eliminado = 1 WHERE IdProveedor = @IdProveedor", con);
                com.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = IdProveedor;
                com.ExecuteNonQuery();

                con.Close();
                return true;
            }
            catch
            {
                con.Close();
                return false;
            }
        }

        #endregion

        #endregion

        #region Pedidos

        #region Select
        public DataTable Select_Pedido_Full()
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdPedido, IdProveedorAux, CIF,IdUsuarioAux, Apodo, IdProductoAux, NombreProducto, PrecioCosto,Pedido.Cantidad,ROUND(CAST ((PrecioCosto * Pedido.Cantidad) AS decimal (30,2)),3) AS Precio, FORMAT(Pedido.FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(Pedido.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Pedido INNER JOIN Proveedor ON Pedido.IdProveedorAux = Proveedor.IdProveedor INNER JOIN Producto ON Pedido.IdProductoAux = Producto.IdProducto INNER JOIN Usuario ON Pedido.IdUsuarioAux = Usuario.IdUsuario ORDER BY IdPedido DESC", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }

        public string[] Select_Pedido_Id(string IdPedido)
        {
            string[] result = new string[9];
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdProveedorAux, CIF, IdProductoAux, NombreProducto, PrecioCosto, Stock, Pedido.Cantidad,ROUND(CAST ((PrecioCosto * Pedido.Cantidad) AS decimal (30,2)),3) AS Precio FROM Pedido INNER JOIN Proveedor ON Pedido.IdProveedorAux = Proveedor.IdProveedor INNER JOIN Producto ON Pedido.IdProductoAux = Producto.IdProducto WHERE IdPedido = @IdPedido", con);
                cmd.Parameters.Add("@IdPedido", SqlDbType.Int).Value = IdPedido;
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dr.Read();
                result[0] = dr["IdProveedorAux"].ToString();
                result[1] = dr["CIF"].ToString();
                result[2] = dr["IdProductoAux"].ToString();
                result[3] = dr["NombreProducto"].ToString();
                result[4] = string.Format(Format, dr["PrecioCosto"].ToString());
                result[5] = dr["Cantidad"].ToString();
                result[6] = dr["Precio"].ToString();
                result[7] = dr["Stock"].ToString();


                dr.Close();
                con.Close();
                return result;
            }
            catch
            {
                con.Close();
                return result;
            }
        }


        #region TextInput
        #region ProductoPedido
        public string Get_IdProducto_ByNombre(string NombreProducto)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdProducto FROM Producto WHERE NombreProducto = @NombreProducto", con);
                cmd.Parameters.Add("@NombreProducto", SqlDbType.VarChar).Value = NombreProducto;
                string IdProducto = (string)cmd.ExecuteScalar();

                con.Close();
                return IdProducto;
            }
            catch
            {
                con.Close();
                return null;
            }
        }

        public string Get_NombreProducto_ByIdProducto(string IdProducto)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT NombreProducto FROM Producto WHERE IdProducto = @IdProducto", con);
                cmd.Parameters.Add("@IdProducto", SqlDbType.Int).Value = IdProducto;
                string NombreProducto = (string)cmd.ExecuteScalar();

                con.Close();
                return NombreProducto;
            }
            catch
            {
                con.Close();
                return null;
            }
        }

        public decimal Get_PrecioCoste_ByIdProducto(string IdProducto)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT ROUND(CAST (PrecioCosto AS decimal (30,2)),3) AS PrecioCosto FROM Producto WHERE IdProducto = @IdProducto", con);
                cmd.Parameters.Add("@IdProducto", SqlDbType.Int).Value = IdProducto;
                decimal PrecioCoste = (decimal)cmd.ExecuteScalar();


                con.Close();
                return PrecioCoste;
            }
            catch
            {
                con.Close();
                return 0;
            }
        }


        public int Get_Stock_ByIdProducto(string IdProducto)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT Stock FROM Producto WHERE IdProducto = @IdProducto", con);
                cmd.Parameters.Add("@IdProducto", SqlDbType.Int).Value = IdProducto;
                int Stock = (int)cmd.ExecuteScalar();

                con.Close();
                return Stock;
            }
            catch
            {
                con.Close();
                return 0;
            }
        }



        public int Get_Stock_ByIdPedido(string IdPedido)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT Stock FROM Pedido INNER JOIN Producto ON Pedido.IdProductoAux = Producto.IdProducto WHERE IdPedido = @IdPedido", con);
                cmd.Parameters.Add("@IdPedido", SqlDbType.Int).Value = IdPedido;
                int Stock = (int)cmd.ExecuteScalar();

                con.Close();
                return Stock;
            }
            catch
            {
                con.Close();
                return 0;
            }
        }


        public int Get_Cantidad_ByIdPedido(string IdPedido)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT Cantidad FROM Pedido WHERE IdPedido = @IdPedido", con);
                cmd.Parameters.Add("@IdPedido", SqlDbType.Int).Value = IdPedido;
                int Cantidad = (int)cmd.ExecuteScalar();

                con.Close();
                return Cantidad;
            }
            catch
            {
                con.Close();
                return 0;
            }
        }
        #endregion

        #region ProveedorPedido
        public string Get_IdProveedor_ByCIF(string CIF)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdProveedor FROM Proveedor WHERE CIF = @CIF", con);
                cmd.Parameters.Add("@CIF", SqlDbType.VarChar).Value = CIF;
                string IdProveedor = (string)cmd.ExecuteScalar();

                con.Close();
                return IdProveedor;
            }
            catch
            {
                con.Close();
                return null;
            }
        }

        public string Get_CIF_ByIdProveedor(string IdProveedor)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT CIF FROM Proveedor WHERE IdProveedor = @IdProveedor", con);
                cmd.Parameters.Add("@IdProveedor", SqlDbType.VarChar).Value = IdProveedor;
                string CIF = (string)cmd.ExecuteScalar();

                con.Close();
                return CIF;
            }
            catch
            {
                con.Close();
                return null;
            }
        }
        #endregion

        #endregion

        #region Filtros Normales

        #region Filtros
        public DataTable Pedido_Id(string IdPedido)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdPedido, IdProveedorAux, CIF,IdUsuarioAux, Apodo, IdProductoAux, NombreProducto, Pedido.Cantidad,ROUND(CAST ((PrecioCosto * Pedido.Cantidad) AS decimal (30,2)),3) AS Precio, FORMAT(Pedido.FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(Pedido.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Pedido INNER JOIN Proveedor ON Pedido.IdProveedorAux = Proveedor.IdProveedor INNER JOIN Producto ON Pedido.IdProductoAux = Producto.IdProducto INNER JOIN Usuario ON Pedido.IdUsuarioAux = Usuario.IdUsuario WHERE IdPedido = @IdPedido ORDER BY IdPedido DESC", con);
                cmd.Parameters.Add("@IdPedido", SqlDbType.Int).Value = IdPedido;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Pedido_Full();
                return TableDefault;
            }
        }

        public DataTable Pedido_CIF(string CIF)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdPedido, IdProveedorAux, CIF,IdUsuarioAux, Apodo, IdProductoAux, NombreProducto, Pedido.Cantidad,ROUND(CAST ((PrecioCosto * Pedido.Cantidad) AS decimal (30,2)),3) AS Precio, FORMAT(Pedido.FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(Pedido.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Pedido INNER JOIN Proveedor ON Pedido.IdProveedorAux = Proveedor.IdProveedor INNER JOIN Producto ON Pedido.IdProductoAux = Producto.IdProducto INNER JOIN Usuario ON Pedido.IdUsuarioAux = Usuario.IdUsuario WHERE CIF LIKE @CIF ORDER BY CIF ASC", con);
                cmd.Parameters.AddWithValue("@CIF", "%" + CIF + "%");
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Pedido_Full();
                return TableDefault;
            }
        }

        public DataTable Pedido_NombreUsuario(string Apodo)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdPedido, IdProveedorAux, CIF,IdUsuarioAux, Apodo, IdProductoAux, NombreProducto, Pedido.Cantidad,ROUND(CAST ((PrecioCosto * Pedido.Cantidad) AS decimal (30,2)),3) AS Precio, FORMAT(Pedido.FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(Pedido.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Pedido INNER JOIN Proveedor ON Pedido.IdProveedorAux = Proveedor.IdProveedor INNER JOIN Producto ON Pedido.IdProductoAux = Producto.IdProducto INNER JOIN Usuario ON Pedido.IdUsuarioAux = Usuario.IdUsuario WHERE Apodo LIKE @Apodo ORDER BY NombreUsuario ASC", con);
                cmd.Parameters.AddWithValue("@Apodo", "%" + Apodo + "%");
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Pedido_Full();
                return TableDefault;
            }
        }


        public DataTable Pedido_NombreProducto(string NombreProducto)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdPedido, IdProveedorAux, CIF,IdUsuarioAux, Apodo, IdProductoAux, NombreProducto, Pedido.Cantidad,ROUND(CAST ((PrecioCosto * Pedido.Cantidad) AS decimal (30,2)),3) AS Precio, FORMAT(Pedido.FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(Pedido.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Pedido INNER JOIN Proveedor ON Pedido.IdProveedorAux = Proveedor.IdProveedor INNER JOIN Producto ON Pedido.IdProductoAux = Producto.IdProducto INNER JOIN Usuario ON Pedido.IdUsuarioAux = Usuario.IdUsuario WHERE NombreProducto LIKE @NombreProducto ORDER BY NombreProducto ASC", con);
                cmd.Parameters.AddWithValue("@NombreProducto", "%" + NombreProducto + "%");
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Pedido_Full();
                return TableDefault;
            }
        }

        public DataTable Pedido_Cantidad(string Cantidad)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdPedido, IdProveedorAux, CIF,IdUsuarioAux, Apodo, IdProductoAux, NombreProducto, Pedido.Cantidad,ROUND(CAST ((PrecioCosto * Pedido.Cantidad) AS decimal (30,2)),3) AS Precio, FORMAT(Pedido.FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(Pedido.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Pedido INNER JOIN Proveedor ON Pedido.IdProveedorAux = Proveedor.IdProveedor INNER JOIN Producto ON Pedido.IdProductoAux = Producto.IdProducto INNER JOIN Usuario ON Pedido.IdUsuarioAux = Usuario.IdUsuario WHERE Pedido.Cantidad LIKE @Cantidad ORDER BY Cantidad ASC", con);
                cmd.Parameters.AddWithValue("@Cantidad", "%" + Cantidad + "%");
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Pedido_Full();
                return TableDefault;
            }
        }

        public DataTable Pedido_Precio(string Precio)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdPedido, IdProveedorAux, CIF,IdUsuarioAux, Apodo, IdProductoAux, NombreProducto, Pedido.Cantidad,ROUND(CAST ((PrecioCosto * Pedido.Cantidad) AS decimal (30,2)),3) AS Precio, FORMAT(Pedido.FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(Pedido.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Pedido INNER JOIN Proveedor ON Pedido.IdProveedorAux = Proveedor.IdProveedor INNER JOIN Producto ON Pedido.IdProductoAux = Producto.IdProducto INNER JOIN Usuario ON Pedido.IdUsuarioAux = Usuario.IdUsuario WHERE Pedido.Precio LIKE @Precio ORDER BY Precio ASC", con);
                cmd.Parameters.AddWithValue("@Precio", "%" + Precio + "%");
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Pedido_Full();
                return TableDefault;
            }
        }

        public DataTable Pedido_FechaModificacion(string FechaModificacion)
        {
            DateTime fecha2 = Convert.ToDateTime(FechaModificacion);
            string FechaConsulta2 = fecha2.AddDays(1).ToString("dd/MM/yyyy");

            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdPedido, IdProveedorAux, CIF,IdUsuarioAux, Apodo, IdProductoAux, NombreProducto, Pedido.Cantidad,ROUND(CAST ((PrecioCosto * Pedido.Cantidad) AS decimal (30,2)),3) AS Precio, FORMAT(Pedido.FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(Pedido.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Pedido INNER JOIN Proveedor ON Pedido.IdProveedorAux = Proveedor.IdProveedor INNER JOIN Producto ON Pedido.IdProductoAux = Producto.IdProducto INNER JOIN Usuario ON Pedido.IdUsuarioAux = Usuario.IdUsuario WHERE Pedido.FechaModificacion BETWEEN '" + FechaModificacion + "' AND '" + FechaConsulta2 + "' ORDER BY Pedido.FechaModificacion DESC", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Pedido_Full();
                return TableDefault;
            }
        }


        public DataTable Pedido_FechaCreacion(string FechaCreacion)
        {
            DateTime fecha2 = Convert.ToDateTime(FechaCreacion);
            string FechaConsulta2 = fecha2.AddDays(1).ToString("dd/MM/yyyy");

            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdPedido, IdProveedorAux, CIF,IdUsuarioAux, Apodo, IdProductoAux, NombreProducto, Pedido.Cantidad,ROUND(CAST ((PrecioCosto * Pedido.Cantidad) AS decimal (30,2)),3) AS Precio, FORMAT(Pedido.FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(Pedido.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Pedido INNER JOIN Proveedor ON Pedido.IdProveedorAux = Proveedor.IdProveedor INNER JOIN Producto ON Pedido.IdProductoAux = Producto.IdProducto INNER JOIN Usuario ON Pedido.IdUsuarioAux = Usuario.IdUsuario WHERE Pedido.FechaCreacion BETWEEN '" + FechaCreacion + "' AND '" + FechaConsulta2 + "' ORDER BY Pedido.FechaCreacion DESC", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Pedido_Full();
                return TableDefault;
            }
        }
        #endregion

        #region Cantidad Precio
        public string[] Gastos_Cantidad_General_Pedido()
        {
            string[] result = new string[12];

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT SUM(Cantidad) AS Cantidad, ROUND(CAST (SUM(Precio) AS decimal (30,2)),3) AS Precio FROM Pedido", con);
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dr.Read();
                result[0] = dr["Precio"].ToString();
                result[1] = dr["Cantidad"].ToString();


                con.Close();
                return result;
            }
            catch
            {
                con.Close();
                return result;
            }
        }


        public string[] Gastos_Cantidad_Apodo_Pedido(string Apodo)
        {
            string[] result = new string[12];

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT SUM(Cantidad) AS Cantidad, ROUND(CAST (SUM(Precio) AS decimal (30,2)),3) AS Precio FROM Pedido INNER JOIN Usuario ON Pedido.IdUsuarioAux = Usuario.IdUsuario WHERE Apodo LIKE @Apodo", con);
                cmd.Parameters.AddWithValue("@Apodo", "%" + Apodo + "%");
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dr.Read();
                result[0] = dr["Precio"].ToString();
                result[1] = dr["Cantidad"].ToString();


                con.Close();
                return result;
            }
            catch
            {
                con.Close();
                return result;
            }
        }

        public string[] Gastos_Cantidad_CIF_Pedido(string CIF)
        {
            string[] result = new string[12];

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT SUM(Cantidad) AS Cantidad, ROUND(CAST (SUM(Precio) AS decimal (30,2)),3) AS Precio FROM Pedido INNER JOIN Proveedor ON Pedido.IdProveedorAux = Proveedor.IdProveedor WHERE CIF LIKE @CIF", con);
                cmd.Parameters.AddWithValue("@CIF", "%" + CIF + "%");
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dr.Read();
                result[0] = dr["Precio"].ToString();
                result[1] = dr["Cantidad"].ToString();


                con.Close();
                return result;
            }
            catch
            {
                con.Close();
                return result;
            }
        }


        public string[] Gastos_Cantidad_NombreProducto_Pedido(string NombreProducto)
        {
            string[] result = new string[12];

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT SUM(Cantidad) AS Cantidad, ROUND(CAST (SUM(Precio) AS decimal (30,2)),3) AS Precio FROM Pedido INNER JOIN Producto ON Pedido.IdProductoAux = Producto.IdProducto WHERE NombreProducto LIKE @NombreProducto", con);
                cmd.Parameters.AddWithValue("@NombreProducto", "%" + NombreProducto + "%");
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dr.Read();
                result[0] = dr["Precio"].ToString();
                result[1] = dr["Cantidad"].ToString();


                con.Close();
                return result;
            }
            catch
            {
                con.Close();
                return result;
            }
        }


        public string[] Gastos_Cantidad_Precio_Pedido(string Precio)
        {
            string[] result = new string[12];

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT SUM(Cantidad) AS Cantidad, ROUND(CAST (SUM(Precio) AS decimal (30,2)),3) AS Precio FROM Pedido WHERE Precio LIKE @Precio", con);
                cmd.Parameters.AddWithValue("@Precio", "%" + Precio + "%");
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dr.Read();
                result[0] = dr["Precio"].ToString();
                result[1] = dr["Cantidad"].ToString();


                con.Close();
                return result;
            }
            catch
            {
                con.Close();
                return result;
            }
        }


        public string[] Gastos_Cantidad_Cantidad_Pedido(string Cantidad)
        {
            string[] result = new string[12];

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT SUM(Cantidad) AS Cantidad, ROUND(CAST (SUM(Precio) AS decimal (30,2)),3) AS Precio FROM Pedido WHERE Cantidad LIKE @Cantidad", con);
                cmd.Parameters.AddWithValue("@Cantidad", "%" + Cantidad + "%");
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dr.Read();
                result[0] = dr["Precio"].ToString();
                result[1] = dr["Cantidad"].ToString();


                con.Close();
                return result;
            }
            catch
            {
                con.Close();
                return result;
            }
        }
        #endregion

        #endregion

        #region Filtros Fechas
        #region Fechas
        public DataTable Select_Pedido_Fecha_Hoy(string FechaConsulta)
        {


            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdPedido, IdProveedorAux, CIF,IdUsuarioAux, Apodo, IdProductoAux, NombreProducto, Pedido.Cantidad,ROUND(CAST ((PrecioCosto * Pedido.Cantidad) AS decimal (30,2)),3) AS Precio, FORMAT(Pedido.FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(Pedido.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Pedido INNER JOIN Proveedor ON Pedido.IdProveedorAux = Proveedor.IdProveedor INNER JOIN Producto ON Pedido.IdProductoAux = Producto.IdProducto INNER JOIN Usuario ON Pedido.IdUsuarioAux = Usuario.IdUsuario WHERE Pedido.FechaCreacion >= '" + FechaConsulta + "' ORDER BY IdPedido DESC", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }


        public DataTable Select_Pedido_Fecha1(string FechaConsulta)
        {

            DateTime fecha2 = Convert.ToDateTime(FechaConsulta);
            string FechaConsulta2 = fecha2.AddDays(1).ToString("dd/MM/yyyy");



            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdPedido, IdProveedorAux, CIF,IdUsuarioAux, Apodo, IdProductoAux, NombreProducto, Pedido.Cantidad,ROUND(CAST ((PrecioCosto * Pedido.Cantidad) AS decimal (30,2)),3) AS Precio, FORMAT(Pedido.FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(Pedido.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Pedido INNER JOIN Proveedor ON Pedido.IdProveedorAux = Proveedor.IdProveedor INNER JOIN Producto ON Pedido.IdProductoAux = Producto.IdProducto INNER JOIN Usuario ON Pedido.IdUsuarioAux = Usuario.IdUsuario WHERE Pedido.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + FechaConsulta2 + "' ORDER BY IdPedido DESC", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }


        public DataTable Select_Pedido_Fecha2(string FechaConsulta, string FechaConsulta2)
        {


            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdPedido, IdProveedorAux, CIF,IdUsuarioAux, Apodo, IdProductoAux, NombreProducto, Pedido.Cantidad,ROUND(CAST ((PrecioCosto * Pedido.Cantidad) AS decimal (30,2)),3) AS Precio, FORMAT(Pedido.FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(Pedido.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Pedido INNER JOIN Proveedor ON Pedido.IdProveedorAux = Proveedor.IdProveedor INNER JOIN Producto ON Pedido.IdProductoAux = Producto.IdProducto INNER JOIN Usuario ON Pedido.IdUsuarioAux = Usuario.IdUsuario WHERE Pedido.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + FechaConsulta2 + "' ORDER BY IdPedido DESC", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }
        #endregion

        #region Gastos y Unidades
        public string[] Gastos_Unidades_Fecha_Hoy(string FechaConsulta)
        {
            string[] result = new string[12];

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT SUM(Cantidad) AS Cantidad, ROUND(CAST (SUM(Precio) AS decimal (30,2)),3) AS Precio FROM Pedido WHERE Pedido.FechaCreacion >= '" + FechaConsulta + "'", con);
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dr.Read();
                result[0] = dr["Precio"].ToString();
                result[1] = dr["Cantidad"].ToString();


                con.Close();
                return result;
            }
            catch
            {
                con.Close();
                return result;
            }
        }


        public string[] Gastos_Unidades_1Fecha(string FechaConsulta)
        {
            string[] result = new string[12];


            DateTime fecha2 = Convert.ToDateTime(FechaConsulta);
            string FechaConsulta2 = fecha2.AddDays(1).ToString("dd/MM/yyyy");

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT SUM(Cantidad) AS Cantidad, ROUND(CAST (SUM(Precio) AS decimal (30,2)),3) AS Precio FROM Pedido WHERE Pedido.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + FechaConsulta2 + "'", con);
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dr.Read();
                result[0] = dr["Precio"].ToString();
                result[1] = dr["Cantidad"].ToString();


                con.Close();
                return result;
            }
            catch
            {
                con.Close();
                return result;
            }
        }

        public string[] Gastos_Unidades_2Fecha(string FechaConsulta, string FechaConsulta2)
        {
            string[] result = new string[12];

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT SUM(Cantidad) AS Cantidad, ROUND(CAST (SUM(Precio) AS decimal (30,2)),3) AS Precio FROM Pedido WHERE Pedido.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + FechaConsulta2 + "'", con);
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dr.Read();
                result[0] = dr["Precio"].ToString();
                result[1] = dr["Cantidad"].ToString();


                con.Close();
                return result;
            }
            catch
            {
                con.Close();
                return result;
            }
        }

        #endregion
        #endregion


        public DataTable Pedido_7Dias()
        {

            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdPedido, IdProveedorAux, CIF,IdUsuarioAux, Apodo, IdProductoAux, NombreProducto, Pedido.Cantidad,ROUND(CAST ((PrecioCosto * Pedido.Cantidad) AS decimal (30,2)),3) AS Precio, FORMAT(Pedido.FechaModificacion, 'dd/MM/yyyy HH:mm:ss') as FechaModificacion, FORMAT(Pedido.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Pedido INNER JOIN Proveedor ON Pedido.IdProveedorAux = Proveedor.IdProveedor INNER JOIN Producto ON Pedido.IdProductoAux = Producto.IdProducto INNER JOIN Usuario ON Pedido.IdUsuarioAux = Usuario.IdUsuario WHERE Pedido.FechaCreacion BETWEEN '" + DateTime.Today.AddDays(-7) + "' AND '" + DateTime.Now + "' ORDER BY Pedido.FechaCreacion DESC", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }


        #endregion

        #region Insert

        public void Insert_Pedido(string IdUsuarioAux, string IdProveedorAux, string IdProductoAux, string Cantidad, string Precio)
        {

            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("INSERT INTO Pedido (IdUsuarioAux, IdProveedorAux, IdProductoAux, Cantidad, Precio) values (@IdUsuarioAux, @IdProveedorAux, @IdProductoAux, @Cantidad, @Precio)", con);
                com.Parameters.Add("@IdUsuarioAux", SqlDbType.VarChar).Value = IdUsuarioAux;
                com.Parameters.Add("@IdProveedorAux", SqlDbType.VarChar).Value = IdProveedorAux;
                com.Parameters.Add("@IdProductoAux", SqlDbType.VarChar).Value = IdProductoAux;
                if (Cantidad == "") { com.Parameters.AddWithValue("@Cantidad", DBNull.Value); } else { com.Parameters.Add("@Cantidad", SqlDbType.Int).Value = Cantidad; }
                if (Precio == "") { com.Parameters.AddWithValue("@Precio", DBNull.Value); } else { com.Parameters.Add("@Precio", SqlDbType.Money).Value = Precio; }
                com.ExecuteNonQuery();

                con.Close();

            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }

        }

        #endregion

        #region Update

        public void Update_IdProveedorAux_Pedido(string IdProveedorAux, string IdPedido)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Pedido SET IdProveedorAux = @IdProveedorAux, FechaModificacion = @FechaModificacion WHERE IdPedido = @IdPedido", con);
                com.Parameters.Add("@IdProveedorAux", SqlDbType.Int).Value = IdProveedorAux;
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdPedido", SqlDbType.Int).Value = IdPedido;
                com.ExecuteNonQuery();


                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }
        }

        public void Update_IdProductoAux_Pedido(string IdProductoAux, string IdPedido)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Pedido SET IdProductoAux = @IdProductoAux, FechaModificacion = @FechaModificacion WHERE IdPedido = @IdPedido", con);
                com.Parameters.Add("@IdProductoAux", SqlDbType.Int).Value = IdProductoAux;
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdPedido", SqlDbType.Int).Value = IdPedido;
                com.ExecuteNonQuery();


                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }
        }

        public void Update_Cantidad_Precio_Pedido(string Cantidad, string Precio, string IdPedido)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("UPDATE Pedido SET Cantidad = @Cantidad, Precio = @Precio, FechaModificacion = @FechaModificacion WHERE IdPedido = @IdPedido", con);
                com.Parameters.Add("@Cantidad", SqlDbType.Int).Value = Cantidad;
                com.Parameters.Add("@Precio", SqlDbType.Money).Value = Precio;
                com.Parameters.Add("@FechaModificacion", SqlDbType.DateTime2).Value = DateTime.Now;
                com.Parameters.Add("@IdPedido", SqlDbType.Int).Value = IdPedido;
                com.ExecuteNonQuery();


                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }
        }


        #endregion

        #region Delete

        public bool Delete_Pedido_Id(string IdPedido)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("DELETE FROM Pedido WHERE IdPedido = @IdPedido", con);
                com.Parameters.Add("@IdPedido", SqlDbType.Int).Value = IdPedido;
                com.ExecuteNonQuery();

                con.Close();
                return true;
            }
            catch
            {
                con.Close();
                return false;
            }
        }

        #endregion

        #endregion

        #region Facturas

        #region Select

        public int VentasSinTerminar()
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT COUNT(IdFactura) FROM Factura WHERE Finalizada = 0", con);
            int Ventas = (int)cmd.ExecuteScalar();

            con.Close();
            return Ventas;
        }

        public int NumVentas()
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Factura INNER JOIN Usuario ON Factura.IdUsuarioAux = Usuario.IdUsuario WHERE Finalizada = 0 AND Factura.FechaCreacion >= '" + DateTime.Now.ToString("dd/MM/yyyy") + "'", con);
            int NumVentas = (int)cmd.ExecuteScalar();

            con.Close();
            return NumVentas;
        }

        public DataTable Fill_Cart()
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdFactura, PrecioTotal, Promocion, FORMAT(Factura.FechaCreacion, 'HH:mm:ss') as FechaCreacion, IdUsuarioAux, NombreUsuario FROM Factura INNER JOIN Usuario ON Factura.IdUsuarioAux = Usuario.IdUsuario WHERE Finalizada = 0 AND Factura.FechaCreacion >= '" + DateTime.Now.ToString("dd/MM/yyyy") + "' ORDER BY Factura.FechaCreacion DESC", con);
            //SqlCommand cmd = new SqlCommand("SELECT IdFactura, PrecioTotal, Promocion, FORMAT(Factura.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion, IdUsuarioAux, NombreUsuario FROM Factura INNER JOIN Usuario ON Factura.IdUsuarioAux = Usuario.IdUsuario WHERE Finalizada = 0 ORDER BY Factura.FechaCreacion DESC", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();
            return dt;
        }


        public DataTable Fill_Detalles(string IdFactura)
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdFactura, PrecioTotal, IdDetalles, Detalles.Cantidad, PrecioCantidad, IdProductoAux, NombreProducto, PrecioVenta, Clasificacion, IVA, Descripcion,Stock FROM Factura INNER JOIN Detalles ON Factura.IdFactura = Detalles.IdFacturaAux  INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto WHERE IdFactura = '" + IdFactura + "'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();
            return dt;
        }


        public bool Update_Current_Venta(string IdProductoAux, string IdFacturaAux)
        {
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Detalles WHERE IdProductoAux = @IdProductoAux AND IdFacturaAux = @IdFacturaAux", con);
                cmd.Parameters.Add("@IdProductoAux", SqlDbType.VarChar).Value = IdProductoAux;
                cmd.Parameters.Add("@IdFacturaAux", SqlDbType.VarChar).Value = IdFacturaAux;
                int num = (int)cmd.ExecuteScalar();
                con.Close();
                if (num >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                con.Close();
                return false;
            }
        }


        public int Get_Count_Detalles(string IdFacturaAux)
        {
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Detalles WHERE IdFacturaAux = 2", con);
                cmd.Parameters.Add("@IdFacturaAux", SqlDbType.VarChar).Value = IdFacturaAux;
                int num = (int)cmd.ExecuteScalar();
                con.Close();

                return num;

            }
            catch
            {
                con.Close();
                return 0;
            }
        }


        public int Get_Detalles_Cantidad_NombreProducto(string NombreProducto, string IdFacturaAux)
        {
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT Detalles.Cantidad FROM Detalles INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto WHERE NombreProducto = @NombreProducto AND IdFacturaAux = @IdFacturaAux", con);
                cmd.Parameters.Add("@NombreProducto", SqlDbType.VarChar).Value = NombreProducto;
                cmd.Parameters.Add("@IdFacturaAux", SqlDbType.Int).Value = IdFacturaAux;
                int num = (int)cmd.ExecuteScalar();
                con.Close();
                if (num >= 1)
                {
                    return num;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                con.Close();
                return 0;
            }
        }

        public decimal Get_Detalles_PrecioCantidad_NombreProducto(string NombreProducto, string IdFacturaAux)
        {
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT PrecioCantidad FROM Detalles INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto WHERE NombreProducto = @NombreProducto AND IdFacturaAux = @IdFacturaAux", con);
                cmd.Parameters.Add("@NombreProducto", SqlDbType.VarChar).Value = NombreProducto;
                cmd.Parameters.Add("@IdFacturaAux", SqlDbType.VarChar).Value = IdFacturaAux;
                decimal num = (decimal)cmd.ExecuteScalar();
                con.Close();
                if (num >= 1)
                {
                    return num;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                con.Close();
                return 0;
            }
        }


        public int Get_Producto_Cantidad(string IdProducto)
        {
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT Cantidad FROM Producto WHERE IdProducto = @IdProducto", con);
                cmd.Parameters.Add("@IdProducto", SqlDbType.VarChar).Value = IdProducto;
                int num = (int)cmd.ExecuteScalar();
                con.Close();
                return num;
            }
            catch
            {
                con.Close();
                return 0;
            }
        }

        public decimal Get_FullPrice_Factura(string IdFacturaAux)
        {
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT SUM(Detalles.Cantidad * PrecioVenta) AS Gastos FROM Detalles INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto WHERE IdFacturaAux = @IdFacturaAux", con);
                cmd.Parameters.Add("@IdFacturaAux", SqlDbType.VarChar).Value = IdFacturaAux;
                decimal num = (decimal)cmd.ExecuteScalar();
                con.Close();
                return num;
            }
            catch
            {
                con.Close();
                return 0;
            }
        }


        public int Get_Promo_Factura(string IdFactura)
        {
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT Promocion FROM Factura WHERE IdFactura = @IdFactura", con);
                cmd.Parameters.Add("@IdFactura", SqlDbType.VarChar).Value = IdFactura;
                int num = (int)cmd.ExecuteScalar();
                con.Close();
                return num;
            }
            catch
            {
                con.Close();
                return 0;
            }
        }


        public string[] Gastos_Beneficio_StockHistorico_General()
        {
            string[] result = new string[12];
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT SUM(Cantidad * PrecioCosto) AS Gastos,SUM(Cantidad * PrecioBeneficio) AS Beneficios, SUM (Detalles.Cantidad) AS StockHistorico FROM Detalles INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto INNER JOIN Factura ON Detalles.IdFacturaAux = Factura.IdFactura", con);
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dr.Read();
                result[0] = string.Format("{0:0,0#}", dr["Gastos"].ToString());
                result[1] = string.Format("{0:0,0#}", dr["Beneficios"].ToString());
                result[2] = dr["StockHistorico"].ToString();
                con.Close();

                result[3] = PrecioTotal_General();

                return result;
            }
            catch
            {
                con.Close();
                return result;
            }
        }

        public string PrecioTotal_General()
        {
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT ROUND(CAST (SUM((PrecioTotal - (PrecioTotal * (Promocion/100.0)))) AS decimal (30,2)),3) FROM Factura", con);
                decimal num = (decimal)cmd.ExecuteScalar();

                con.Close();
                return num.ToString();
            }
            catch
            {
                con.Close();
                return "0";
            }
        }

        public string[] Gastos_Beneficio_StockHistorico_Fecha_Hoy(string FechaConsulta)
        {
            string[] result = new string[12];

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT SUM(Cantidad * PrecioCosto) AS Gastos,SUM(Cantidad * PrecioBeneficio) AS Beneficios, SUM (Detalles.Cantidad) AS StockHistorico FROM Detalles INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto INNER JOIN Factura ON Detalles.IdFacturaAux = Factura.IdFactura WHERE Factura.FechaCreacion >= '" + FechaConsulta + "'", con);
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dr.Read();
                result[0] = string.Format("{0:0,0#}", dr["Gastos"].ToString());
                result[1] = string.Format(Format, dr["Beneficios"].ToString());
                result[2] = dr["StockHistorico"].ToString();

                con.Close();

                result[3] = PrecioTotal_Fecha_Hoy(FechaConsulta);

                return result;
            }
            catch
            {
                con.Close();
                return result;
            }
        }

        public string PrecioTotal_Fecha_Hoy(string FechaConsulta)
        {
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT ROUND(CAST (SUM((PrecioTotal - (PrecioTotal * (Promocion/100.0)))) AS decimal (30,2)),3) FROM Factura  WHERE Factura.FechaCreacion >= '" + FechaConsulta + "'", con);
                decimal num = (decimal)cmd.ExecuteScalar();

                con.Close();
                return num.ToString();
            }
            catch
            {
                con.Close();
                return "0";
            }
        }


        public string[] Gastos_Beneficio_StockHistorico_1Fecha(string FechaConsulta)
        {
            string[] result = new string[12];


            DateTime fecha2 = Convert.ToDateTime(FechaConsulta);
            string FechaConsulta2 = fecha2.AddDays(1).ToString("dd/MM/yyyy");

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT SUM(Cantidad * PrecioCosto) AS Gastos,SUM(Cantidad * PrecioBeneficio) AS Beneficios, SUM (Detalles.Cantidad) AS StockHistorico FROM Detalles INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto INNER JOIN Factura ON Detalles.IdFacturaAux = Factura.IdFactura WHERE Factura.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + FechaConsulta2 + "'", con);
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dr.Read();
                result[0] = string.Format("{0:0,0#}", dr["Gastos"].ToString());
                result[1] = string.Format(Format, dr["Beneficios"].ToString());
                result[2] = dr["StockHistorico"].ToString();

                con.Close();

                result[3] = PrecioTotal_1Fecha(FechaConsulta);

                return result;
            }
            catch
            {
                con.Close();
                return result;
            }
        }

        public string PrecioTotal_1Fecha(string FechaConsulta)
        {
            DateTime fecha2 = Convert.ToDateTime(FechaConsulta);
            string FechaConsulta2 = fecha2.AddDays(1).ToString("dd/MM/yyyy");

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT ROUND(CAST (SUM((PrecioTotal - (PrecioTotal * (Promocion/100.0)))) AS decimal (30,2)),3) FROM Factura WHERE Factura.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + FechaConsulta2 + "'", con);
                decimal num = (decimal)cmd.ExecuteScalar();

                con.Close();
                return num.ToString();
            }
            catch
            {
                con.Close();
                return "0";
            }
        }



        public string[] Gastos_Beneficio_StockHistorico_2Fecha(string FechaConsulta, string FechaConsulta2)
        {
            string[] result = new string[12];

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT SUM(Cantidad * PrecioCosto) AS Gastos,SUM(Cantidad * PrecioBeneficio) AS Beneficios, SUM (Detalles.Cantidad) AS StockHistorico FROM Detalles INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto INNER JOIN Factura ON Detalles.IdFacturaAux = Factura.IdFactura WHERE Factura.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + FechaConsulta2 + "'", con);
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dr.Read();
                result[0] = string.Format("{0:0,0#}", dr["Gastos"].ToString());
                result[1] = string.Format(Format, dr["Beneficios"].ToString());
                result[2] = dr["StockHistorico"].ToString();

                con.Close();

                result[3] = PrecioTotal_2Fecha(FechaConsulta, FechaConsulta2);

                return result;
            }
            catch
            {
                con.Close();
                return result;
            }
        }

        public string PrecioTotal_2Fecha(string FechaConsulta, string FechaConsulta2)
        {

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT ROUND(CAST (SUM((PrecioTotal - (PrecioTotal * (Promocion/100.0)))) AS decimal (30,2)),3) FROM Factura WHERE Factura.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + FechaConsulta2 + "'", con);
                decimal num = (decimal)cmd.ExecuteScalar();

                con.Close();
                return num.ToString();
            }
            catch
            {
                con.Close();
                return "0";
            }
        }





        public DataTable Gastos_Beneficio_StockHistorico_Producto()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT SUM(Cantidad * PrecioCosto) AS Gastos,SUM(Cantidad * PrecioBeneficio) AS Beneficios, SUM (Detalles.Cantidad) AS StockHistorico FROM Detalles INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto INNER JOIN Factura ON Detalles.IdFacturaAux = Factura.IdFactura", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();
            return dt;
        }

        public DataTable Gastos_Beneficio_StockHistorico_Producto_Fecha_Hoy(string FechaConsulta)
        {

            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT SUM(Cantidad * PrecioCosto) AS Gastos,SUM(Cantidad * PrecioBeneficio) AS Beneficios, SUM (Detalles.Cantidad) AS StockHistorico FROM Detalles INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto INNER JOIN Factura ON Detalles.IdFacturaAux = Factura.IdFactura WHERE Factura.FechaCreacion >= '" + FechaConsulta + "'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);


            con.Close();
            return dt;
        }


        public DataTable Gastos_Beneficio_StockHistorico_Producto_1Fecha(string FechaConsulta)
        {
            DateTime fecha2 = Convert.ToDateTime(FechaConsulta);

            fecha2 = fecha2.AddDays(1);

            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT SUM(Cantidad * PrecioCosto) AS Gastos,SUM(Cantidad * PrecioBeneficio) AS Beneficios, SUM (Detalles.Cantidad) AS StockHistorico FROM Detalles INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto INNER JOIN Factura ON Detalles.IdFacturaAux = Factura.IdFactura WHERE Factura.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + fecha2 + "'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);


            con.Close();
            return dt;
        }

        public DataTable Gastos_Beneficio_StockHistorico_Producto_2Fecha(string FechaConsulta, string FechaConsulta2)
        {

            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT SUM(Cantidad * PrecioCosto) AS Gastos,SUM(Cantidad * PrecioBeneficio) AS Beneficios, SUM (Detalles.Cantidad) AS StockHistorico FROM Detalles INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto INNER JOIN Factura ON Detalles.IdFacturaAux = Factura.IdFactura WHERE Factura.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + FechaConsulta2 + "'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);


            con.Close();
            return dt;
        }


        /// <summary>
        /// /////////////////////////////
        /// </summary>
        /// <returns></returns>

        public DataTable Select_Factura_Full()
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdFactura, IdUsuarioAux, ROUND(CAST (PrecioTotal AS decimal (30,2)),3) AS PrecioTotal,Promocion, ROUND(CAST ((PrecioTotal - (PrecioTotal * (Promocion/100.0))) AS decimal (30,2)),3) AS PrecioPromocion, FORMAT(Factura.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion, Apodo FROM Factura INNER JOIN Usuario ON Factura.IdUsuarioAux = Usuario.IdUsuario ORDER BY IdFactura DESC", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }


        public DataTable Select_Detalles_Full(string IdFactura)
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdDetalles, IdProductoAux, NombreProducto, PrecioCosto, PrecioBeneficio, PrecioVenta, IVA, Detalles.Cantidad, PrecioCosto * Detalles.Cantidad AS Gastos, PrecioBeneficio * Detalles.Cantidad AS Beneficios, PrecioVenta * Detalles.Cantidad AS PrecioFinal FROM Detalles INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto WHERE IdFacturaAux = '" + IdFactura + "' ORDER BY NombreProducto", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }

        #region Filtro Fechas

        public DataTable Select_Factura_Fecha_Hoy(string FechaConsulta)
        {


            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdFactura, IdUsuarioAux, ROUND(CAST (PrecioTotal AS decimal (30,2)),3) AS PrecioTotal,Promocion, ROUND(CAST ((PrecioTotal - (PrecioTotal * (Promocion/100.0))) AS decimal (30,2)),3) AS PrecioPromocion, FORMAT(Factura.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion, Apodo FROM Factura INNER JOIN Usuario ON Factura.IdUsuarioAux = Usuario.IdUsuario WHERE Factura.FechaCreacion >= '" + FechaConsulta + "' ORDER BY IdFactura DESC", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }


        public DataTable Select_Factura_Fecha1(string FechaConsulta)
        {

            DateTime fecha2 = Convert.ToDateTime(FechaConsulta);
            string FechaConsulta2 = fecha2.AddDays(1).ToString("dd/MM/yyyy");



            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdFactura, IdUsuarioAux, ROUND(CAST (PrecioTotal AS decimal (30,2)),3) AS PrecioTotal,Promocion, ROUND(CAST ((PrecioTotal - (PrecioTotal * (Promocion/100.0))) AS decimal (30,2)),3) AS PrecioPromocion, FORMAT(Factura.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion, Apodo FROM Factura INNER JOIN Usuario ON Factura.IdUsuarioAux = Usuario.IdUsuario WHERE Factura.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + FechaConsulta2 + "' ORDER BY IdFactura DESC", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }


        public DataTable Select_Factura_Fecha2(string FechaConsulta, string FechaConsulta2)
        {


            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdFactura, IdUsuarioAux, ROUND(CAST (PrecioTotal AS decimal (30,2)),3) AS PrecioTotal,Promocion, ROUND(CAST ((PrecioTotal - (PrecioTotal * (Promocion/100.0))) AS decimal (30,2)),3) AS PrecioPromocion, FORMAT(Factura.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion, Apodo FROM Factura INNER JOIN Usuario ON Factura.IdUsuarioAux = Usuario.IdUsuario WHERE Factura.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + FechaConsulta2 + "' ORDER BY IdFactura DESC", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }
        #endregion

        #region Filtro Apodo e ID
        public DataTable Select_Factura_Apodo(string Apodo)
        {


            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdFactura, IdUsuarioAux, ROUND(CAST (PrecioTotal AS decimal (30,2)),3) AS PrecioTotal,Promocion, ROUND(CAST ((PrecioTotal - (PrecioTotal * (Promocion/100.0))) AS decimal (30,2)),3) AS PrecioPromocion, FORMAT(Factura.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion, Apodo FROM Factura INNER JOIN Usuario ON Factura.IdUsuarioAux = Usuario.IdUsuario WHERE Apodo LIKE @Apodo ORDER BY IdFactura DESC", con);
            cmd.Parameters.AddWithValue("@Apodo", "%" + Apodo + "%");
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }

        public string[] Gastos_Beneficio_StockHistorico_Apodo(string Apodo)
        {
            string[] result = new string[12];

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT SUM(Cantidad * PrecioCosto) AS Gastos,SUM(Cantidad * PrecioBeneficio) AS Beneficios, SUM (Detalles.Cantidad) AS StockHistorico, ROUND(CAST (SUM((PrecioTotal - (PrecioTotal * (Promocion/100.0)))) AS decimal (30,2)),3) AS PrecioTotalPromocion FROM Detalles INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto INNER JOIN Factura ON Detalles.IdFacturaAux = Factura.IdFactura INNER JOIN Usuario ON Factura.IdUsuarioAux = Usuario.IdUsuario WHERE Apodo LIKE @Apodo", con);
                cmd.Parameters.AddWithValue("@Apodo", "%" + Apodo + "%");
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dr.Read();
                result[0] = string.Format("{0:0,0#}", dr["Gastos"].ToString());
                result[1] = string.Format(Format, dr["Beneficios"].ToString());
                result[2] = dr["StockHistorico"].ToString();
                con.Close();

                result[3] = PrecioTotal_Apodo(Apodo);

                return result;
            }
            catch
            {
                con.Close();
                return result;
            }
        }

        public string PrecioTotal_Apodo(string Apodo)
        {
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT ROUND(CAST (SUM((PrecioTotal - (PrecioTotal * (Promocion/100.0)))) AS decimal (30,2)),3) FROM Factura INNER JOIN Usuario ON Factura.IdUsuarioAux = Usuario.IdUsuario WHERE Apodo LIKE @Apodo", con);
                cmd.Parameters.AddWithValue("@Apodo", "%" + Apodo + "%");
                decimal num = (decimal)cmd.ExecuteScalar();

                con.Close();
                return num.ToString();
            }
            catch
            {
                con.Close();
                return "0";
            }
        }





        public DataTable Select_Factura_IdFactura(string IdFactura)
        {


            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdFactura, IdUsuarioAux, ROUND(CAST (PrecioTotal AS decimal (30,2)),3) AS PrecioTotal,Promocion, ROUND(CAST ((PrecioTotal - (PrecioTotal * (Promocion/100.0))) AS decimal (30,2)),3) AS PrecioPromocion, FORMAT(Factura.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion, Apodo FROM Factura INNER JOIN Usuario ON Factura.IdUsuarioAux = Usuario.IdUsuario WHERE IdFactura LIKE @IdFactura ORDER BY IdFactura DESC", con);
            cmd.Parameters.Add("@IdFactura", SqlDbType.Int).Value = IdFactura;
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }

        public string[] Gastos_Beneficio_StockHistorico_IdFactura(string IdFactura)
        {
            string[] result = new string[12];

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT SUM(Cantidad * PrecioCosto) AS Gastos,SUM(Cantidad * PrecioBeneficio) AS Beneficios, SUM (Detalles.Cantidad) AS StockHistorico, ROUND(CAST (SUM((PrecioTotal - (PrecioTotal * (Promocion/100.0)))) AS decimal (30,2)),3) AS PrecioTotalPromocion FROM Detalles INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto INNER JOIN Factura ON Detalles.IdFacturaAux = Factura.IdFactura WHERE IdFactura LIKE @IdFactura", con);
                cmd.Parameters.Add("@IdFactura", SqlDbType.Int).Value = IdFactura;
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dr.Read();
                result[0] = string.Format("{0:0,0#}", dr["Gastos"].ToString());
                result[1] = string.Format(Format, dr["Beneficios"].ToString());
                result[2] = dr["StockHistorico"].ToString();
                result[2] = dr["StockHistorico"].ToString();
                con.Close();

                result[3] = PrecioTotal_IdFactura(IdFactura);

                return result;
            }
            catch
            {
                con.Close();
                return result;
            }
        }



        public string PrecioTotal_IdFactura(string IdFactura)
        {
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT ROUND(CAST (SUM((PrecioTotal - (PrecioTotal * (Promocion/100.0)))) AS decimal (30,2)),3) FROM Factura INNER JOIN Usuario ON Factura.IdUsuarioAux = Usuario.IdUsuario WHERE IdFactura = @IdFactura", con);
                cmd.Parameters.Add("@IdFactura", SqlDbType.Int).Value = IdFactura;
                decimal num = (decimal)cmd.ExecuteScalar();

                con.Close();
                return num.ToString();
            }
            catch
            {
                con.Close();
                return "0";
            }
        }


        #endregion

        #region StockHistorico
        public DataTable StockHistorico()
        {

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT NombreProducto, SUM(Cantidad * PrecioCosto) AS Gastos ,SUM(Cantidad * PrecioBeneficio) AS Beneficios, SUM (Detalles.Cantidad) AS StockHistorico FROM Detalles INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto GROUP BY NombreProducto", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);


                con.Close();
                return dt;
            }
            catch
            {
                con.Close();
                return null;
            }
        }

        #region Categorias
        public DataTable StockHistorico_Categoria(string Categoria)
        {

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT NombreProducto, SUM(Cantidad * PrecioCosto) AS Gastos ,SUM(Cantidad * PrecioBeneficio) AS Beneficios, SUM (Detalles.Cantidad) AS StockHistorico FROM Detalles INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto WHERE Categoria = @Categoria GROUP BY NombreProducto", con);
                cmd.Parameters.Add("@Categoria", SqlDbType.VarChar).Value = Categoria;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);


                con.Close();
                return dt;
            }
            catch
            {
                con.Close();
                return null;
            }
        }


        public DataTable StockHistorico_Fav()
        {

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT NombreProducto, SUM(Cantidad * PrecioCosto) AS Gastos ,SUM(Cantidad * PrecioBeneficio) AS Beneficios, SUM (Detalles.Cantidad) AS StockHistorico FROM Detalles INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto WHERE Favorito = 1 GROUP BY NombreProducto", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);


                con.Close();
                return dt;
            }
            catch
            {
                con.Close();
                return null;
            }
        }
        #endregion

        #region Fechas
        public DataTable StockHistorico_Hoy(string FechaConsulta)
        {


            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT NombreProducto, SUM(Cantidad * PrecioCosto) AS Gastos ,SUM(Cantidad * PrecioBeneficio) AS Beneficios, SUM (Detalles.Cantidad) AS StockHistorico FROM Detalles INNER JOIN Factura ON Detalles.IdFacturaAux = Factura.IdFactura  INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto WHERE Factura.FechaCreacion >= '" + FechaConsulta + "' GROUP BY NombreProducto", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }

        public DataTable StockHistorico_Fecha1(string FechaConsulta)
        {

            DateTime fecha2 = Convert.ToDateTime(FechaConsulta);
            string FechaConsulta2 = fecha2.AddDays(1).ToString("dd/MM/yyyy");



            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT NombreProducto, SUM(Cantidad * PrecioCosto) AS Gastos ,SUM(Cantidad * PrecioBeneficio) AS Beneficios, SUM (Detalles.Cantidad) AS StockHistorico FROM Detalles INNER JOIN Factura ON Detalles.IdFacturaAux = Factura.IdFactura  INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto WHERE Factura.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + FechaConsulta2 + "' GROUP BY NombreProducto", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }


        public DataTable StockHistorico_Fecha2(string FechaConsulta, string FechaConsulta2)
        {


            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT NombreProducto, SUM(Cantidad * PrecioCosto) AS Gastos ,SUM(Cantidad * PrecioBeneficio) AS Beneficios, SUM (Detalles.Cantidad) AS StockHistorico FROM Detalles INNER JOIN Factura ON Detalles.IdFacturaAux = Factura.IdFactura  INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto WHERE Factura.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + FechaConsulta2 + "' GROUP BY NombreProducto", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }
        #endregion


        #region Fechas Categorias
        public DataTable StockHistorico_Hoy_Categoria(string FechaConsulta, string Categoria)
        {


            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT NombreProducto, SUM(Cantidad * PrecioCosto) AS Gastos ,SUM(Cantidad * PrecioBeneficio) AS Beneficios, SUM (Detalles.Cantidad) AS StockHistorico FROM Detalles INNER JOIN Factura ON Detalles.IdFacturaAux = Factura.IdFactura  INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto WHERE Factura.FechaCreacion >= '" + FechaConsulta + "' AND Categoria = '" + Categoria + "' GROUP BY NombreProducto", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }

        public DataTable StockHistorico_Fecha1_Categoria(string FechaConsulta, string Categoria)
        {

            DateTime fecha2 = Convert.ToDateTime(FechaConsulta);
            string FechaConsulta2 = fecha2.AddDays(1).ToString("dd/MM/yyyy");



            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT NombreProducto, SUM(Cantidad * PrecioCosto) AS Gastos ,SUM(Cantidad * PrecioBeneficio) AS Beneficios, SUM (Detalles.Cantidad) AS StockHistorico FROM Detalles INNER JOIN Factura ON Detalles.IdFacturaAux = Factura.IdFactura  INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto WHERE Factura.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + FechaConsulta2 + "' AND Categoria = '" + Categoria + "' GROUP BY NombreProducto", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }


        public DataTable StockHistorico_Fecha2_Categoria(string FechaConsulta, string FechaConsulta2, string Categoria)
        {


            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT NombreProducto, SUM(Cantidad * PrecioCosto) AS Gastos ,SUM(Cantidad * PrecioBeneficio) AS Beneficios, SUM (Detalles.Cantidad) AS StockHistorico FROM Detalles INNER JOIN Factura ON Detalles.IdFacturaAux = Factura.IdFactura  INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto WHERE Factura.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + FechaConsulta2 + "' AND Categoria = '" + Categoria + "' GROUP BY NombreProducto", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }
        #endregion


        #endregion


        #region Filtros Juntados

        #region Filtro Fechas Apodo

        public DataTable Select_Factura_Fecha_Hoy_Apodo(string FechaConsulta, string Apodo)
        {


            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdFactura, IdUsuarioAux, ROUND(CAST (PrecioTotal AS decimal (30,2)),3) AS PrecioTotal,Promocion, ROUND(CAST ((PrecioTotal - (PrecioTotal * (Promocion/100.0))) AS decimal (30,2)),3) AS PrecioPromocion, FORMAT(Factura.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion, Apodo FROM Factura INNER JOIN Usuario ON Factura.IdUsuarioAux = Usuario.IdUsuario WHERE Apodo LIKE @Apodo AND Factura.FechaCreacion >= '" + FechaConsulta + "' ORDER BY IdFactura DESC", con);
            cmd.Parameters.AddWithValue("@Apodo", "%" + Apodo + "%");
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }


        public DataTable Select_Factura_Fecha1_Apodo(string FechaConsulta, string Apodo)
        {

            DateTime fecha2 = Convert.ToDateTime(FechaConsulta);
            string FechaConsulta2 = fecha2.AddDays(1).ToString("dd/MM/yyyy");

            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdFactura, IdUsuarioAux, ROUND(CAST (PrecioTotal AS decimal (30,2)),3) AS PrecioTotal,Promocion, ROUND(CAST ((PrecioTotal - (PrecioTotal * (Promocion/100.0))) AS decimal (30,2)),3) AS PrecioPromocion, FORMAT(Factura.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion, Apodo FROM Factura INNER JOIN Usuario ON Factura.IdUsuarioAux = Usuario.IdUsuario WHERE Apodo LIKE @Apodo AND Factura.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + FechaConsulta2 + "' ORDER BY IdFactura DESC", con);
            cmd.Parameters.AddWithValue("@Apodo", "%" + Apodo + "%");
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }


        public DataTable Select_Factura_Fecha2_Apodo(string FechaConsulta, string FechaConsulta2, string Apodo)
        {


            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdFactura, IdUsuarioAux, ROUND(CAST (PrecioTotal AS decimal (30,2)),3) AS PrecioTotal,Promocion, ROUND(CAST ((PrecioTotal - (PrecioTotal * (Promocion/100.0))) AS decimal (30,2)),3) AS PrecioPromocion, FORMAT(Factura.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion, Apodo FROM Factura INNER JOIN Usuario ON Factura.IdUsuarioAux = Usuario.IdUsuario WHERE Apodo LIKE @Apodo AND Factura.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + FechaConsulta2 + "' ORDER BY IdFactura DESC", con);
            cmd.Parameters.AddWithValue("@Apodo", "%" + Apodo + "%");
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }
        #endregion

        #region Filtro Fechas Id

        public DataTable Select_Factura_Fecha_Hoy_Id(string FechaConsulta, string IdFactura)
        {


            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdFactura, IdUsuarioAux, ROUND(CAST (PrecioTotal AS decimal (30,2)),3) AS PrecioTotal,Promocion, ROUND(CAST ((PrecioTotal - (PrecioTotal * (Promocion/100.0))) AS decimal (30,2)),3) AS PrecioPromocion, FORMAT(Factura.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion, Apodo FROM Factura INNER JOIN Usuario ON Factura.IdUsuarioAux = Usuario.IdUsuario WHERE IdFactura = @IdFactura AND Factura.FechaCreacion >= '" + FechaConsulta + "' ORDER BY IdFactura DESC", con);
            cmd.Parameters.Add("@IdFactura", SqlDbType.VarChar).Value = IdFactura;
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }


        public DataTable Select_Factura_Fecha1_Id(string FechaConsulta, string IdFactura)
        {

            DateTime fecha2 = Convert.ToDateTime(FechaConsulta);
            string FechaConsulta2 = fecha2.AddDays(1).ToString("dd/MM/yyyy");

            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdFactura, IdUsuarioAux, ROUND(CAST (PrecioTotal AS decimal (30,2)),3) AS PrecioTotal,Promocion, ROUND(CAST ((PrecioTotal - (PrecioTotal * (Promocion/100.0))) AS decimal (30,2)),3) AS PrecioPromocion, FORMAT(Factura.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion, Apodo FROM Factura INNER JOIN Usuario ON Factura.IdUsuarioAux = Usuario.IdUsuario WHERE IdFactura = @IdFactura AND Factura.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + FechaConsulta2 + "' ORDER BY IdFactura DESC", con);
            cmd.Parameters.Add("@IdFactura", SqlDbType.Int).Value = IdFactura;
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }


        public DataTable Select_Factura_Fecha2_Id(string FechaConsulta, string FechaConsulta2, string IdFactura)
        {


            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdFactura, IdUsuarioAux, ROUND(CAST (PrecioTotal AS decimal (30,2)),3) AS PrecioTotal,Promocion, ROUND(CAST ((PrecioTotal - (PrecioTotal * (Promocion/100.0))) AS decimal (30,2)),3) AS PrecioPromocion, FORMAT(Factura.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion, Apodo FROM Factura INNER JOIN Usuario ON Factura.IdUsuarioAux = Usuario.IdUsuario WHERE IdFactura = @IdFactura AND Factura.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + FechaConsulta2 + "' ORDER BY IdFactura DESC", con);
            cmd.Parameters.Add("@IdFactura", SqlDbType.Int).Value = IdFactura;
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }
        #endregion


        #region StockHistorico

        #region Fechas Apodo
        public string[] Gastos_Beneficio_StockHistorico_Fecha_Hoy_Apodo(string FechaConsulta, string Apodo)
        {
            string[] result = new string[12];

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT SUM(Cantidad * PrecioCosto) AS Gastos,SUM(Cantidad * PrecioBeneficio) AS Beneficios, SUM (Detalles.Cantidad) AS StockHistorico FROM Detalles INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto INNER JOIN Factura ON Detalles.IdFacturaAux = Factura.IdFactura INNER JOIN Usuario ON Factura.IdUsuarioAux = Usuario.IdUsuario WHERE Apodo LIKE @Apodo AND Factura.FechaCreacion >= '" + FechaConsulta + "'", con);
                cmd.Parameters.AddWithValue("@Apodo", "%" + Apodo + "%");
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dr.Read();
                result[0] = string.Format("{0:0,0#}", dr["Gastos"].ToString());
                result[1] = string.Format(Format, dr["Beneficios"].ToString());
                result[2] = dr["StockHistorico"].ToString();

                con.Close();

                result[3] = PrecioTotal_Fecha_Hoy_Apodo(FechaConsulta, Apodo);

                return result;
            }
            catch
            {
                con.Close();
                return result;
            }
        }

        public string PrecioTotal_Fecha_Hoy_Apodo(string FechaConsulta, string Apodo)
        {
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT ROUND(CAST (SUM((PrecioTotal - (PrecioTotal * (Promocion/100.0)))) AS decimal (30,2)),3) FROM Factura INNER JOIN Usuario ON Factura.IdUsuarioAux = Usuario.IdUsuario WHERE Apodo LIKE @Apodo AND Factura.FechaCreacion >= '" + FechaConsulta + "'", con);
                cmd.Parameters.AddWithValue("@Apodo", "%" + Apodo + "%");
                decimal num = (decimal)cmd.ExecuteScalar();

                con.Close();
                return num.ToString();
            }
            catch
            {
                con.Close();
                return "0";
            }
        }


        public string[] Gastos_Beneficio_StockHistorico_1Fecha_Apodo(string FechaConsulta, string Apodo)
        {
            string[] result = new string[12];


            DateTime fecha2 = Convert.ToDateTime(FechaConsulta);
            string FechaConsulta2 = fecha2.AddDays(1).ToString("dd/MM/yyyy");

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT SUM(Cantidad * PrecioCosto) AS Gastos,SUM(Cantidad * PrecioBeneficio) AS Beneficios, SUM (Detalles.Cantidad) AS StockHistorico FROM Detalles INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto INNER JOIN Factura ON Detalles.IdFacturaAux = Factura.IdFactura INNER JOIN Usuario ON Factura.IdUsuarioAux = Usuario.IdUsuario WHERE Apodo LIKE @Apodo AND Factura.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + FechaConsulta2 + "'", con);
                cmd.Parameters.AddWithValue("@Apodo", "%" + Apodo + "%");
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dr.Read();
                result[0] = string.Format("{0:0,0#}", dr["Gastos"].ToString());
                result[1] = string.Format(Format, dr["Beneficios"].ToString());
                result[2] = dr["StockHistorico"].ToString();

                con.Close();

                result[3] = PrecioTotal_1Fecha_Apodo(FechaConsulta, Apodo);

                return result;
            }
            catch
            {
                con.Close();
                return result;
            }
        }

        public string PrecioTotal_1Fecha_Apodo(string FechaConsulta, string Apodo)
        {
            DateTime fecha2 = Convert.ToDateTime(FechaConsulta);
            string FechaConsulta2 = fecha2.AddDays(1).ToString("dd/MM/yyyy");

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT ROUND(CAST (SUM((PrecioTotal - (PrecioTotal * (Promocion/100.0)))) AS decimal (30,2)),3) FROM Factura INNER JOIN Usuario ON Factura.IdUsuarioAux = Usuario.IdUsuario WHERE Apodo LIKE @Apodo AND Factura.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + FechaConsulta2 + "'", con);
                cmd.Parameters.AddWithValue("@Apodo", "%" + Apodo + "%");
                decimal num = (decimal)cmd.ExecuteScalar();

                con.Close();
                return num.ToString();
            }
            catch
            {
                con.Close();
                return "0";
            }
        }



        public string[] Gastos_Beneficio_StockHistorico_2Fecha_Apodo(string FechaConsulta, string FechaConsulta2, string Apodo)
        {
            string[] result = new string[12];

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT SUM(Cantidad * PrecioCosto) AS Gastos,SUM(Cantidad * PrecioBeneficio) AS Beneficios, SUM (Detalles.Cantidad) AS StockHistorico FROM Detalles INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto INNER JOIN Factura ON Detalles.IdFacturaAux = Factura.IdFactura INNER JOIN Usuario ON Factura.IdUsuarioAux = Usuario.IdUsuario WHERE Apodo LIKE @Apodo AND Factura.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + FechaConsulta2 + "'", con);
                cmd.Parameters.AddWithValue("@Apodo", "%" + Apodo + "%");
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dr.Read();
                result[0] = string.Format("{0:0,0#}", dr["Gastos"].ToString());
                result[1] = string.Format(Format, dr["Beneficios"].ToString());
                result[2] = dr["StockHistorico"].ToString();

                con.Close();

                result[3] = PrecioTotal_2Fecha_Apodo(FechaConsulta, FechaConsulta2, Apodo);

                return result;
            }
            catch
            {
                con.Close();
                return result;
            }
        }

        public string PrecioTotal_2Fecha_Apodo(string FechaConsulta, string FechaConsulta2, string Apodo)
        {

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT ROUND(CAST (SUM((PrecioTotal - (PrecioTotal * (Promocion/100.0)))) AS decimal (30,2)),3) FROM Factura INNER JOIN Usuario ON Factura.IdUsuarioAux = Usuario.IdUsuario WHERE Apodo LIKE @Apodo AND Factura.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + FechaConsulta2 + "'", con);
                cmd.Parameters.AddWithValue("@Apodo", "%" + Apodo + "%");
                decimal num = (decimal)cmd.ExecuteScalar();

                con.Close();
                return num.ToString();
            }
            catch
            {
                con.Close();
                return "0";
            }
        }
        #endregion

        #region Fechas Id
        public string[] Gastos_Beneficio_StockHistorico_Fecha_Hoy_IdFactura(string FechaConsulta, string IdFactura)
        {
            string[] result = new string[12];

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT SUM(Cantidad * PrecioCosto) AS Gastos,SUM(Cantidad * PrecioBeneficio) AS Beneficios, SUM (Detalles.Cantidad) AS StockHistorico FROM Detalles INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto INNER JOIN Factura ON Detalles.IdFacturaAux = Factura.IdFactura WHERE Apodo = @Apodo AND Factura.FechaCreacion >= '" + FechaConsulta + "'", con);
                cmd.Parameters.Add("@IdFactura", SqlDbType.Int).Value = IdFactura;
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dr.Read();
                result[0] = string.Format("{0:0,0#}", dr["Gastos"].ToString());
                result[1] = string.Format(Format, dr["Beneficios"].ToString());
                result[2] = dr["StockHistorico"].ToString();

                con.Close();

                result[3] = PrecioTotal_Fecha_Hoy_IdFactura(FechaConsulta, IdFactura);

                return result;
            }
            catch
            {
                con.Close();
                return result;
            }
        }

        public string PrecioTotal_Fecha_Hoy_IdFactura(string FechaConsulta, string IdFactura)
        {
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT ROUND(CAST (SUM((PrecioTotal - (PrecioTotal * (Promocion/100.0)))) AS decimal (30,2)),3) FROM Factura WHERE Apodo = @Apodo AND Factura.FechaCreacion >= '" + FechaConsulta + "'", con);
                cmd.Parameters.Add("@IdFactura", SqlDbType.Int).Value = IdFactura;
                decimal num = (decimal)cmd.ExecuteScalar();

                con.Close();
                return num.ToString();
            }
            catch
            {
                con.Close();
                return "0";
            }
        }


        public string[] Gastos_Beneficio_StockHistorico_1Fecha_IdFactura(string FechaConsulta, string IdFactura)
        {
            string[] result = new string[12];


            DateTime fecha2 = Convert.ToDateTime(FechaConsulta);
            string FechaConsulta2 = fecha2.AddDays(1).ToString("dd/MM/yyyy");

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT SUM(Cantidad * PrecioCosto) AS Gastos,SUM(Cantidad * PrecioBeneficio) AS Beneficios, SUM (Detalles.Cantidad) AS StockHistorico FROM Detalles INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto INNER JOIN Factura ON Detalles.IdFacturaAux = Factura.IdFactura WHERE Apodo = @Apodo AND Factura.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + FechaConsulta2 + "'", con);
                cmd.Parameters.Add("@IdFactura", SqlDbType.Int).Value = IdFactura;
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dr.Read();
                result[0] = string.Format("{0:0,0#}", dr["Gastos"].ToString());
                result[1] = string.Format(Format, dr["Beneficios"].ToString());
                result[2] = dr["StockHistorico"].ToString();

                con.Close();

                result[3] = PrecioTotal_1Fecha_IdFactura(FechaConsulta, IdFactura);

                return result;
            }
            catch
            {
                con.Close();
                return result;
            }
        }

        public string PrecioTotal_1Fecha_IdFactura(string FechaConsulta, string IdFactura)
        {
            DateTime fecha2 = Convert.ToDateTime(FechaConsulta);
            string FechaConsulta2 = fecha2.AddDays(1).ToString("dd/MM/yyyy");

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT ROUND(CAST (SUM((PrecioTotal - (PrecioTotal * (Promocion/100.0)))) AS decimal (30,2)),3) FROM Factura WHERE Apodo = @Apodo AND Factura.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + FechaConsulta2 + "'", con);
                cmd.Parameters.Add("@IdFactura", SqlDbType.Int).Value = IdFactura;
                decimal num = (decimal)cmd.ExecuteScalar();

                con.Close();
                return num.ToString();
            }
            catch
            {
                con.Close();
                return "0";
            }
        }



        public string[] Gastos_Beneficio_StockHistorico_2Fecha_IdFactura(string FechaConsulta, string FechaConsulta2, string IdFactura)
        {
            string[] result = new string[12];

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT SUM(Cantidad * PrecioCosto) AS Gastos,SUM(Cantidad * PrecioBeneficio) AS Beneficios, SUM (Detalles.Cantidad) AS StockHistorico FROM Detalles INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto INNER JOIN Factura ON Detalles.IdFacturaAux = Factura.IdFactura WHERE Apodo = @Apodo AND Factura.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + FechaConsulta2 + "'", con);
                cmd.Parameters.Add("@IdFactura", SqlDbType.Int).Value = IdFactura;
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dr.Read();
                result[0] = string.Format("{0:0,0#}", dr["Gastos"].ToString());
                result[1] = string.Format(Format, dr["Beneficios"].ToString());
                result[2] = dr["StockHistorico"].ToString();

                con.Close();

                result[3] = PrecioTotal_2Fecha_IdFactura(FechaConsulta, FechaConsulta2, IdFactura);

                return result;
            }
            catch
            {
                con.Close();
                return result;
            }
        }

        public string PrecioTotal_2Fecha_IdFactura(string FechaConsulta, string FechaConsulta2, string IdFactura)
        {

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT ROUND(CAST (SUM((PrecioTotal - (PrecioTotal * (Promocion/100.0)))) AS decimal (30,2)),3) FROM Factura WHERE Apodo = @Apodo AND Factura.FechaCreacion BETWEEN '" + FechaConsulta + "' AND '" + FechaConsulta2 + "'", con);
                cmd.Parameters.Add("@IdFactura", SqlDbType.Int).Value = IdFactura;
                decimal num = (decimal)cmd.ExecuteScalar();

                con.Close();
                return num.ToString();
            }
            catch
            {
                con.Close();
                return "0";
            }
        }
        #endregion


        #endregion

        #endregion


        public DataTable Ticket_Factura(string IdFactura)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdFactura, Apodo,ROUND(CAST (PrecioTotal AS decimal (30,2)),3) AS PrecioTotal,Promocion, ROUND(CAST ((PrecioTotal - (PrecioTotal * (Promocion/100.0))) AS decimal (30,2)),3) AS PrecioPromocion, FORMAT(Factura.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion FROM Factura INNER JOIN Usuario ON Factura.IdUsuarioAux = Usuario.IdUsuario WHERE IdFactura = @IdFactura", con);
                cmd.Parameters.Add("@IdFactura", SqlDbType.Int).Value = IdFactura;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                return null;
            }


        }

        public DataTable Ticket_Detalles(string IdFacturaAux)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT NombreProducto, ROUND(CAST (PrecioVenta AS decimal (30,2)),3) AS PrecioVenta, IVA, Clasificacion, Detalles.Cantidad FROM Detalles INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto WHERE IdFacturaAux = '" + IdFacturaAux + "' ORDER BY NombreProducto", con);
                cmd.Parameters.Add("@IdFacturaAux", SqlDbType.Int).Value = IdFacturaAux;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                return null;
            }


        }


        public int Get_Detalles_Cantidad(string IdDetalles)
        {
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT Cantidad FROM Detalles WHERE IdDetalles = @IdDetalles", con);
                cmd.Parameters.Add("@IdDetalles", SqlDbType.Int).Value = IdDetalles;
                int num = (int)cmd.ExecuteScalar();
                con.Close();
                return num;
            }
            catch
            {
                con.Close();
                return 0;
            }
        }

        public int Get_Stock_NombreProducto(string NombreProducto)
        {
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT Stock FROM Producto WHERE NombreProducto = @NombreProducto", con);
                cmd.Parameters.Add("@NombreProducto", SqlDbType.VarChar).Value = NombreProducto;
                int num = (int)cmd.ExecuteScalar();
                con.Close();
                return num;
            }
            catch
            {
                con.Close();
                return 0;
            }
        }


        public DataTable Ventas_Hoy()
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdFactura, Apodo, ROUND(CAST (PrecioTotal AS decimal (30,2)),3) AS PrecioTotal,Promocion, ROUND(CAST ((PrecioTotal - (PrecioTotal * (Promocion/100.0))) AS decimal (30,2)),3) AS PrecioPromocion, FORMAT(Factura.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion, IdUsuarioAux, NombreUsuario FROM Factura INNER JOIN Usuario ON Factura.IdUsuarioAux = Usuario.IdUsuario WHERE Finalizada = 1 AND Factura.FechaCreacion >= '" + DateTime.Now.ToString("dd/MM/yyyy") + "' ORDER BY Factura.FechaCreacion DESC", con);
            //SqlCommand cmd = new SqlCommand("SELECT IdFactura, PrecioTotal, Promocion, FORMAT(Factura.FechaCreacion, 'dd/MM/yyyy HH:mm:ss') as FechaCreacion, IdUsuarioAux, NombreUsuario FROM Factura INNER JOIN Usuario ON Factura.IdUsuarioAux = Usuario.IdUsuario WHERE Finalizada = '" + 1 + "' ORDER BY Factura.FechaCreacion DESC", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();
            return dt;
        }


        public string[] Datos_Hoy()
        {
            string[] result = new string[12];

            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT SUM(Cantidad * PrecioCosto) AS Gastos,SUM(Cantidad * PrecioBeneficio) AS Beneficios, SUM (Detalles.Cantidad) AS StockHistorico FROM Detalles INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto INNER JOIN Factura ON Detalles.IdFacturaAux = Factura.IdFactura WHERE Finalizada = 1 AND Factura.FechaCreacion >= '" + DateTime.Now.ToString("dd/MM/yyyy") + "'", con);
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dr.Read();
                result[0] = string.Format("{0:0,0#}", dr["Gastos"].ToString());
                result[1] = string.Format(Format, dr["Beneficios"].ToString());
                result[2] = dr["StockHistorico"].ToString();

                con.Close();

                result[3] = PrecioTotal_Hoy();

                return result;
            }
            catch
            {
                con.Close();
                return result;
            }
        }

        public string PrecioTotal_Hoy()
        {
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT ROUND(CAST (SUM((PrecioTotal - (PrecioTotal * (Promocion/100.0)))) AS decimal (30,2)),3) FROM Factura WHERE Finalizada = 1 AND Factura.FechaCreacion >= '" + DateTime.Now.ToString("dd/MM/yyyy") + "'", con);
                decimal num = (decimal)cmd.ExecuteScalar();

                con.Close();
                return num.ToString();
            }
            catch
            {
                con.Close();
                return "0";
            }
        }

        #endregion

        #region Insert

        public int Insert_Factura()
        {
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand("INSERT INTO Factura (IdUsuarioAux) OUTPUT INSERTED.IdFactura VALUES (@IdUsuarioAux)", con);
                com.Parameters.Add("@IdUsuarioAux", SqlDbType.VarChar).Value = Global.IdUsuarioLog;
                int IdFactura = (int)com.ExecuteScalar();

                con.Close();

                return IdFactura;
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
                return 0;
            }

        }


        public void Insert_Detalles(string IdProductoAux, string Cantidad, string PrecioCantidad, string IdFacturaAux)
        {
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand("INSERT INTO Detalles (IdProductoAux, Cantidad, PrecioCantidad, IdFacturaAux) VALUES (@IdProductoAux, @Cantidad, @PrecioCantidad, @IdFacturaAux)", con);
                com.Parameters.Add("@IdProductoAux", SqlDbType.VarChar).Value = IdProductoAux;
                com.Parameters.Add("@Cantidad", SqlDbType.Int).Value = Cantidad;
                com.Parameters.Add("@PrecioCantidad", SqlDbType.Money).Value = PrecioCantidad;
                com.Parameters.Add("@IdFacturaAux", SqlDbType.VarChar).Value = IdFacturaAux;
                com.ExecuteNonQuery();
                con.Close();

            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }

        }
        #endregion

        #region Update
        public void Update_Factura_PrecioTotal(string PrecioTotal, string IdFactura)
        {
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand("UPDATE Factura SET PrecioTotal = @PrecioTotal WHERE IdFactura = @IdFactura", con);
                com.Parameters.Add("@PrecioTotal", SqlDbType.Money).Value = PrecioTotal;
                com.Parameters.Add("@IdFactura", SqlDbType.VarChar).Value = IdFactura;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }

        }



        public void Update_Factura_Promocion(string Promocion, string IdFactura)
        {
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand("UPDATE Factura SET Promocion = @Promocion WHERE IdFactura = @IdFactura", con);
                com.Parameters.Add("@Promocion", SqlDbType.Int).Value = Promocion;
                com.Parameters.Add("@IdFactura", SqlDbType.VarChar).Value = IdFactura;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }

        }



        public void Update_Factura_Finalizada(string Finalizada, string IdFactura)
        {
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand("UPDATE Factura SET Finalizada = @Finalizada WHERE IdFactura = @IdFactura", con);
                com.Parameters.Add("@Finalizada", SqlDbType.Int).Value = Finalizada;
                com.Parameters.Add("@IdFactura", SqlDbType.VarChar).Value = IdFactura;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }

        }



        public void Update_Detalles_Cantidad(string Cantidad, string IdDetalles)
        {
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand("UPDATE Detalles SET Cantidad = @Cantidad WHERE IdDetalles = @IdDetalles", con);
                com.Parameters.Add("@Cantidad", SqlDbType.Int).Value = Cantidad;
                com.Parameters.Add("@IdDetalles", SqlDbType.VarChar).Value = IdDetalles;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }

        }

        public void Update_Detalles_PrecioCantidad(string PrecioCantidad, string IdDetalles)
        {
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand("UPDATE Detalles SET PrecioCantidad = @PrecioCantidad WHERE IdDetalles = @IdDetalles", con);
                com.Parameters.Add("@PrecioCantidad", SqlDbType.Money).Value = PrecioCantidad;
                com.Parameters.Add("@IdDetalles", SqlDbType.VarChar).Value = IdDetalles;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }

        }








        public void Update_Detalles_Cantidad_NombreProducto(string Cantidad, string NombreProducto, string IdFacturaAux)
        {
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand("UPDATE Detalles SET Detalles.Cantidad = @Cantidad From Detalles INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto WHERE NombreProducto = @NombreProducto AND IdFacturaAux = @IdFacturaAux", con);
                com.Parameters.Add("@Cantidad", SqlDbType.Int).Value = Cantidad;
                com.Parameters.Add("@NombreProducto", SqlDbType.VarChar).Value = NombreProducto;
                com.Parameters.Add("@IdFacturaAux", SqlDbType.VarChar).Value = IdFacturaAux;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }

        }

        public void Update_Detalles_PrecioCantidad_NombreProducto(string PrecioCantidad, string NombreProducto, string IdFacturaAux)
        {
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand("UPDATE Detalles SET PrecioCantidad = @PrecioCantidad From Detalles INNER JOIN Producto ON Detalles.IdProductoAux = Producto.IdProducto WHERE NombreProducto = @NombreProducto AND IdFacturaAux = @IdFacturaAux", con);
                com.Parameters.Add("@PrecioCantidad", SqlDbType.Money).Value = PrecioCantidad;
                com.Parameters.Add("@NombreProducto", SqlDbType.VarChar).Value = NombreProducto;
                com.Parameters.Add("@IdFacturaAux", SqlDbType.VarChar).Value = IdFacturaAux;
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }

        }

        public void Update_Facturas_Antiguas()
        {

            try
            {
                con.Open();
                SqlCommand com = new SqlCommand("UPDATE Factura SET Finalizada = 1 From Factura WHERE Finalizada = 0", con);
                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }
        }


        #endregion

        #region Delete
        public bool Delete_Factura_Id(string IdFactura)
        {
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("DELETE FROM Factura WHERE IdFactura = @IdFactura", con);
                com.Parameters.Add("@IdFactura", SqlDbType.VarChar).Value = IdFactura;
                com.ExecuteNonQuery();

                con.Close();
                return true;
            }
            catch
            {
                con.Close();
                return false;
            }
        }



        public bool Delete_Detalles_Id(string IdDetalles)
        {
            try
            {
                con.Open();

                SqlCommand com2 = new SqlCommand("DELETE FROM Detalles WHERE IdDetalles = @IdDetalles", con);
                com2.Parameters.Add("@IdDetalles", SqlDbType.VarChar).Value = IdDetalles;
                com2.ExecuteNonQuery();


                con.Close();
                return true;
            }
            catch
            {
                con.Close();
                return false;
            }
        }


        #endregion

        #endregion





        #region Database
        public static bool CheckDatabaseExists()
        {
            bool result = false;
            SqlConnection tmpConn = new SqlConnection(@"server=.;Trusted_Connection=yes");
            try
            {
                using (SqlCommand sqlCmd = new SqlCommand("SELECT database_id FROM sys.databases WHERE Name = 'ProyectoDAM'", tmpConn))
                {
                    tmpConn.Open();
                    object resultObj = sqlCmd.ExecuteScalar();
                    int databaseID = 0;

                    if (resultObj != null)
                    {
                        int.TryParse(resultObj.ToString(), out databaseID);
                    }
                    tmpConn.Close();
                    result = (databaseID > 0);
                }
            }
            catch (Exception)
            {
                tmpConn.Close();
                result = false;
            }
            return result;
        }

        public bool DB()
        {
            return CheckDatabaseExists();
        }


        public bool CreateDatabase()
        {
            String CreateDatabase;
            SqlConnection connection = new SqlConnection(@"server=.;Trusted_Connection=yes");
            string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            GrantAccess(appPath);
            bool IsExits = CheckDatabaseExists(); 
                if (!IsExits)
                {
                CreateDatabase = "CREATE DATABASE ProyectoDAM;";
                SqlCommand command = new SqlCommand(CreateDatabase, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
                    return false;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }

                CreateTables();

                return true;
                }
               
            return false;

        }

        public void CreateTables()
        {
            if (!CreateTableUsuario())
            {
                MessageBox.Show("Usuario no creado");
            }
            if (!CreateTableProducto())
            {
                MessageBox.Show("Producto no creado");
            }
            if (!CreateTableProveedor())
            {
                MessageBox.Show("Proveedor no creado");
            }
            if (!CreateTablePedido())
            {
                MessageBox.Show("Pedido no creado");
            }
            if (!CreateTableFactura())
            {
                MessageBox.Show("Factura no creado");
            }
            if (!CreateTableDetalles())
            {
                MessageBox.Show("Detalles no creado");
            }
        }


        public bool CreateTableUsuario()
        {
            try
            {
                con.Open();
                SqlCommand com1 = new SqlCommand("CREATE TABLE Usuario(IdUsuario INT PRIMARY KEY IDENTITY (1,1) NOT NULL,DNI VARCHAR(9) NOT NULL,Apodo VARCHAR(50) NOT NULL,Contrasena VARBINARY (500) NOT NULL,Privilegio VARCHAR(20) DEFAULT 'Usuario',NombreUsuario VARCHAR(50) NOT NULL,Apellido VARCHAR(50) NOT NULL,Telefono INT NOT NULL,FechaNac DATE,Email VARCHAR(100) NOT NULL,PFP IMAGE,Recordar BIT DEFAULT 0,PrimerLog BIT DEFAULT 0,Eliminado BIT DEFAULT 0,FechaCreacion DATETIME DEFAULT GETDATE(),FechaModificacion DATETIME DEFAULT GETDATE())", con);
                com1.ExecuteNonQuery();

                con.Close();
                return true;
            }
            catch (Exception)
            {
                con.Close();
                return false;
            }
        }

        public bool CreateTableProducto()
        {
            try
            {
                con.Open();

                SqlCommand com2 = new SqlCommand("CREATE TABLE Producto(IdProducto INT PRIMARY KEY IDENTITY (1,1) NOT NULL,Clasificacion VARCHAR (20) DEFAULT 'B',NombreProducto VARCHAR (50) NOT NULL,PrecioCosto MONEY NOT NULL,PrecioBeneficio MONEY NOT NULL,PrecioVenta MONEY NOT NULL,IVA INT DEFAULT 10,Stock INT DEFAULT 0,Categoria VARCHAR (50) DEFAULT 'Otros',Favorito BIT DEFAULT 0,Img IMAGE,Ingredientes VARCHAR(300),Descripcion VARCHAR(300),Eliminado BIT DEFAULT 0,FechaCreacion DATETIME DEFAULT GETDATE(),FechaModificacion DATETIME DEFAULT GETDATE())", con);
                com2.ExecuteNonQuery();

                con.Close();
                return true;
            }
            catch (Exception)
            {
                con.Close();
                return false;
            }
        }
        public bool CreateTableProveedor()
        {
            try
            {
                con.Open();

                SqlCommand com3 = new SqlCommand("CREATE TABLE Proveedor(IdProveedor INT PRIMARY KEY IDENTITY (1,1) NOT NULL,CIF VARCHAR (9) NOT NULL,NombreProveedor VARCHAR (50) NOT NULL,Telefono INT NOT NULL,Telefono2 INT,Email VARCHAR (100) NOT NULL,Poblacion VARCHAR (50) NOT NULL,CodPostal VARCHAR (10) NOT NULL,Direccion VARCHAR (100),Descripcion VARCHAR (200),Eliminado BIT DEFAULT 0,FechaCreacion DATETIME DEFAULT GETDATE(),FechaModificacion DATETIME DEFAULT GETDATE())", con);
                com3.ExecuteNonQuery();

                con.Close();
                return true;
            }
            catch (Exception)
            {
                con.Close();
                return false;
            }
        }
        public bool CreateTablePedido()
        {
            try
            {
                con.Open();

                SqlCommand com4 = new SqlCommand("CREATE TABLE Pedido(IdPedido INT PRIMARY KEY IDENTITY (1,1) NOT NULL,IdUsuarioAux INT NOT NULL,CONSTRAINT FK_IdUsuario_Pedido FOREIGN KEY (IdUsuarioAux) REFERENCES Usuario(IdUsuario),IdProveedorAux INT NOT NULL,CONSTRAINT FK_IdProveedor_Pedido FOREIGN KEY (IdProveedorAux) REFERENCES Proveedor(IdProveedor),IdProductoAux INT NOT NULL,CONSTRAINT FK_IdProducto_Pedido FOREIGN KEY (IdProductoAux) REFERENCES Producto(IdProducto),Cantidad INT NOT NULL,Precio MONEY NOT NULL,FechaCreacion DATETIME DEFAULT GETDATE(),FechaModificacion DATETIME DEFAULT GETDATE())", con);
                com4.ExecuteNonQuery();

                con.Close();
                return true;
            }
            catch (Exception)
            {
                con.Close();
                return false;
            }
        }
        public bool CreateTableFactura()
        {
            try
            {
                con.Open();

                SqlCommand com5 = new SqlCommand("CREATE TABLE Factura(IdFactura INT PRIMARY KEY IDENTITY (1,1) NOT NULL,IdUsuarioAux INT NOT NULL,CONSTRAINT FK_IdUsuario_Factura FOREIGN KEY (IdUsuarioAux) REFERENCES Usuario(IdUsuario),PrecioTotal MONEY,Promocion INT DEFAULT 0,Finalizada BIT DEFAULT 0,FechaCreacion DATETIME2 DEFAULT GETDATE())", con);
                com5.ExecuteNonQuery();

                con.Close();
                return true;
            }
            catch (Exception)
            {
                con.Close();
                return false;
            }
        }
        public bool CreateTableDetalles()
        {
            try
            {
                con.Open();

                SqlCommand com6 = new SqlCommand("CREATE TABLE Detalles(IdDetalles INT PRIMARY KEY IDENTITY (1,1) NOT NULL,IdProductoAux INT NOT NULL,CONSTRAINT FK_IdProducto_Detalles FOREIGN KEY (IdProductoAux) REFERENCES Producto(IdProducto),Cantidad INT NOT NULL,PrecioCantidad MONEY NOT NULL,IdFacturaAux INT NOT NULL,CONSTRAINT FK_IdFactura_Detalles FOREIGN KEY (IdFacturaAux) REFERENCES Factura(IdFactura),)", con);
                com6.ExecuteNonQuery();

                con.Close();
                return true;
            }
            catch (Exception)
            {
                con.Close();
                return false;
            }
        }

        public static bool GrantAccess(string fullPath)
        {
            DirectoryInfo info = new DirectoryInfo(fullPath);
            WindowsIdentity self = System.Security.Principal.WindowsIdentity.GetCurrent();
            DirectorySecurity ds = info.GetAccessControl();
            ds.AddAccessRule(new FileSystemAccessRule(self.Name,
            FileSystemRights.FullControl,
            InheritanceFlags.ObjectInherit |
            InheritanceFlags.ContainerInherit,
            PropagationFlags.None,
            AccessControlType.Allow));
            info.SetAccessControl(ds);
            return true;
        }

        #endregion

        #region Clientes

        #region Select
        public DataTable Select_Cliente_Full()
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT IdCliente, NombrePersona, Apellido, DNI, FechaNac, Email, Telefono, Sexo, IdPersonaAux, Puntos, Direccion FROM Cliente INNER JOIN Persona ON Cliente.IdPersonaAux = Persona.IdPersona ORDER BY IdCliente ASC", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            return dt;
        }

        public string[] Select_Cliente_Id(string IdCliente)
        {
            string[] result = new string[9];
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM Cliente INNER JOIN Persona ON Cliente.IdPersonaAux = Persona.IdPersona WHERE IdCliente = @IdCliente", con);
                cmd.Parameters.Add("@IdCliente", SqlDbType.Int).Value = IdCliente;
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dr.Read();
                result[0] = dr["NombrePersona"].ToString();
                result[1] = dr["Apellido"].ToString();
                result[2] = dr["DNI"].ToString();
                result[3] = dr["Telefono"].ToString();
                result[4] = dr["Email"].ToString();
                result[5] = dr["Sexo"].ToString();
                result[6] = dr["Direccion"].ToString();
                result[7] = dr["FechaNac"].ToString();
                result[8] = dr["Puntos"].ToString();

                dr.Close();
                con.Close();
                return result;
            }
            catch
            {
                con.Close();
                return result;
            }
        }



        public DataTable Cliente_Id(string IdCliente)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdCliente, NombrePersona, Apellido, DNI, FechaNac, Email, Telefono, Sexo, IdPersonaAux, Puntos, Direccion FROM Cliente INNER JOIN Persona ON Cliente.IdPersonaAux = Persona.IdPersona WHERE IdCliente = @IdCliente ORDER BY IdCliente ASC", con);
                cmd.Parameters.Add("@IdCliente", SqlDbType.Int).Value = IdCliente;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Cliente_Full();
                return TableDefault;
            }


        }

        public DataTable Cliente_Nombre(string NombrePersona)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdCliente, NombrePersona, Apellido, DNI, FechaNac, Email, Telefono, Sexo, IdPersonaAux, Puntos, Direccion FROM Cliente INNER JOIN Persona ON Cliente.IdPersonaAux = Persona.IdPersona WHERE NombrePersona = @NombrePersona ORDER BY IdCliente ASC", con);
                cmd.Parameters.Add("@NombrePersona", SqlDbType.VarChar).Value = NombrePersona;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Cliente_Full();
                return TableDefault;
            }
        }

        public DataTable Cliente_Apellido(string Apellido)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdCliente, NombrePersona, Apellido, DNI, FechaNac, Email, Telefono, Sexo, IdPersonaAux, Puntos, Direccion FROM Cliente INNER JOIN Persona ON Cliente.IdPersonaAux = Persona.IdPersona WHERE Apellido = @Apellido ORDER BY IdCliente ASC", con);
                cmd.Parameters.Add("@Apellido", SqlDbType.VarChar).Value = Apellido;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Cliente_Full();
                return TableDefault;
            }
        }

        public DataTable Cliente_DNI(string DNI)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdCliente, NombrePersona, Apellido, DNI, FechaNac, Email, Telefono, Sexo, IdPersonaAux, Puntos, Direccion FROM Cliente INNER JOIN Persona ON Cliente.IdPersonaAux = Persona.IdPersona WHERE DNI = @DNI ORDER BY IdCliente ASC", con);
                cmd.Parameters.Add("@DNI", SqlDbType.VarChar).Value = DNI;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Cliente_Full();
                return TableDefault;
            }
        }

        public DataTable Cliente_FechaNac(string FechaNac)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdCliente, NombrePersona, Apellido, DNI, FechaNac, Email, Telefono, Sexo, IdPersonaAux, Puntos, Direccion FROM Cliente INNER JOIN Persona ON Cliente.IdPersonaAux = Persona.IdPersona WHERE FechaNac = @FechaNac ORDER BY IdCliente ASC", con);
                cmd.Parameters.Add("@FechaNac", SqlDbType.Date).Value = FechaNac;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Cliente_Full();
                return TableDefault;
            }
        }

        public DataTable Cliente_Telefono(string Telefono)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdCliente, NombrePersona, Apellido, DNI, FechaNac, Email, Telefono, Sexo, IdPersonaAux, Puntos, Direccion FROM Cliente INNER JOIN Persona ON Cliente.IdPersonaAux = Persona.IdPersona WHERE Telefono = @Telefono ORDER BY IdCliente ASC", con);
                cmd.Parameters.Add("@Telefono", SqlDbType.Int).Value = Telefono;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Cliente_Full();
                return TableDefault;
            }
        }

        public DataTable Cliente_Email(string Email)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdCliente, NombrePersona, Apellido, DNI, FechaNac, Email, Telefono, Sexo, IdPersonaAux, Puntos, Direccion FROM Cliente INNER JOIN Persona ON Cliente.IdPersonaAux = Persona.IdPersona WHERE Email = @Email ORDER BY IdCliente ASC", con);
                cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = Email;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Cliente_Full();
                return TableDefault;
            }
        }

        public DataTable Cliente_Direccion(string Direccion)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdCliente, NombrePersona, Apellido, DNI, FechaNac, Email, Telefono, Sexo, IdPersonaAux, Puntos, Direccion FROM Cliente INNER JOIN Persona ON Cliente.IdPersonaAux = Persona.IdPersona WHERE Direccion = @Direccion ORDER BY IdCliente ASC", con);
                cmd.Parameters.Add("@Direccion", SqlDbType.VarChar).Value = Direccion;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Cliente_Full();
                return TableDefault;
            }
        }

        public DataTable Cliente_Puntos(string Puntos)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdCliente, NombrePersona, Apellido, DNI, FechaNac, Email, Telefono, Sexo, IdPersonaAux, Puntos, Direccion FROM Cliente INNER JOIN Persona ON Cliente.IdPersonaAux = Persona.IdPersona WHERE Puntos = @Puntos ORDER BY IdCliente ASC", con);
                cmd.Parameters.Add("@Puntos", SqlDbType.Int).Value = Puntos;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt;
            }
            catch
            {
                con.Close();
                TableDefault = Select_Cliente_Full();
                return TableDefault;
            }
        }

        #endregion

        #region Insert

        public void Insert_Cliente(string NombrePersona, string Apellido, string DNI, string FechaNac, string Email, string Telefono, string Sexo, string Puntos, string Direccion)
        {

            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("INSERT INTO Persona (NombrePersona, Apellido, DNI, FechaNac, Email, Telefono, Sexo) values (@NombrePersona, @Apellido, @DNI, @FechaNac, @Email, @Telefono, @Sexo)", con);
                com.Parameters.Add("@NombrePersona", SqlDbType.VarChar).Value = NombrePersona as string;
                com.Parameters.Add("@Apellido", SqlDbType.VarChar).Value = Apellido as string;
                com.Parameters.Add("@DNI", SqlDbType.VarChar).Value = DNI;
                if (FechaNac == "") { com.Parameters.AddWithValue("@FechaNac", DBNull.Value); } else { com.Parameters.Add("@FechaNac", SqlDbType.Date).Value = FechaNac; }
                com.Parameters.Add("@Email", SqlDbType.VarChar).Value = Email as string;
                if (Telefono == "") { com.Parameters.AddWithValue("@Telefono", DBNull.Value); } else { com.Parameters.Add("@Telefono", SqlDbType.Int).Value = Telefono; }
                if (string.IsNullOrEmpty(Sexo)) { com.Parameters.AddWithValue("@Sexo", DBNull.Value); } else { com.Parameters.Add("@Sexo", SqlDbType.VarChar).Value = Sexo; }
                com.ExecuteNonQuery();


                SqlCommand cmd = new SqlCommand("SELECT IdPersona FROM Persona WHERE DNI  = @DNI", con);
                cmd.Parameters.Add("@DNI", SqlDbType.VarChar).Value = DNI;
                int IdPersonaAux = (int)cmd.ExecuteScalar();

                SqlCommand com2 = new SqlCommand("INSERT INTO Cliente (IdPersonaAux, Puntos, Direccion) OUTPUT Inserted.IdCliente values (@IdPersonaAux, @Puntos, @Direccion)", con);
                com2.Parameters.Add("@IdPersonaAux", SqlDbType.Int).Value = IdPersonaAux;
                com2.Parameters.Add("@Puntos", SqlDbType.Int).Value = Puntos;
                com2.Parameters.Add("@Direccion", SqlDbType.VarChar).Value = Direccion;
                com2.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                MessageBox.Show("sfasfsafasf" + ex.ToString());
            }

        }

        #endregion

        #region Update
        public void Update_Cliente(string NombrePersona, string Apellido, string DNI, string FechaNac, string Email, string Telefono, string Sexo, string Puntos, string Direccion, string OldDNI)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdPersona FROM Persona WHERE DNI = @DNI", con);
                cmd.Parameters.Add("@DNI", SqlDbType.VarChar).Value = OldDNI;
                int IdPersonaAux = (int)cmd.ExecuteScalar();

                SqlCommand com = new SqlCommand("UPDATE Persona SET NombrePersona = @NombrePersona, Apellido = @Apellido, DNI = @DNI, FechaNac = @FechaNac, Email = @Email, Telefono = @Telefono, Sexo = @Sexo WHERE IdPersona = @IdPersona", con);
                com.Parameters.Add("@NombrePersona", SqlDbType.VarChar).Value = NombrePersona as string;
                com.Parameters.Add("@Apellido", SqlDbType.VarChar).Value = Apellido as string;
                com.Parameters.Add("@DNI", SqlDbType.VarChar).Value = DNI;
                if (FechaNac == "") { com.Parameters.AddWithValue("@FechaNac", DBNull.Value); }
                else { com.Parameters.Add("@FechaNac", SqlDbType.Date).Value = FechaNac; }
                com.Parameters.Add("@Email", SqlDbType.VarChar).Value = Email as string;
                if (Telefono == "") { com.Parameters.AddWithValue("@Telefono", DBNull.Value); } else { com.Parameters.Add("@Telefono", SqlDbType.Int).Value = Telefono; }
                if (string.IsNullOrEmpty(Sexo)) { com.Parameters.AddWithValue("@Sexo", DBNull.Value); } else { com.Parameters.Add("@Sexo", SqlDbType.VarChar).Value = Sexo; }
                com.Parameters.Add("@IdPersona", SqlDbType.Int).Value = IdPersonaAux;
                com.ExecuteNonQuery();



                SqlCommand com2 = new SqlCommand("UPDATE Cliente SET Puntos = @Puntos, Direccion = @Direccion WHERE IdPersonaAux = @IdPersonaAux", con);
                com2.Parameters.Add("@Puntos", SqlDbType.Int).Value = Puntos;
                com2.Parameters.Add("@Direccion", SqlDbType.VarChar).Value = Direccion as string;
                com2.Parameters.Add("@IdPersonaAux", SqlDbType.Int).Value = IdPersonaAux;
                com2.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                /*MessageBox.Show(ex.ToString());*/ MessageBox.Show("Error de conexión con la base de datos");
            }
        }
        #endregion

        #region Delete
        public bool Delete_Cliente(string DNI)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT IdPersona FROM Persona WHERE DNI = @DNI", con);
                cmd.Parameters.Add("@DNI", SqlDbType.VarChar).Value = DNI;
                int IdPersonaAux = (int)cmd.ExecuteScalar();


                SqlCommand com = new SqlCommand("DELETE FROM Cliente WHERE IdPersonaAux = @IdPersonaAux", con);
                com.Parameters.Add("@IdPersonaAux", SqlDbType.Int).Value = IdPersonaAux;
                com.ExecuteNonQuery();

                SqlCommand com2 = new SqlCommand("DELETE FROM Persona WHERE DNI = @DNI", con);
                com2.Parameters.Add("@DNI", SqlDbType.VarChar).Value = DNI;
                com2.ExecuteNonQuery();

                con.Close();
                return true;
            }
            catch
            {
                con.Close();
                return false;
            }




        }

        #endregion

        #endregion

    }
}
