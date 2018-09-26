using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Globalization;

namespace controlFallos
{

    class validaciones
    {
        public String folio = "";
        conexion c = new conexion();
        public void Sololetras(KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                MessageBox.Show("Sólo se Aceptan Letras En Este Campo", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Handled = true;
            }

        }
        public void Solonumeros(KeyPressEventArgs pE)
        {
            if (char.IsDigit(pE.KeyChar))
            {
                pE.Handled = false;
            }
            else if (char.IsControl(pE.KeyChar))
            {
                pE.Handled = false;
            }
            else
            {
                MessageBox.Show("Sólo Se Aceptan Números En Este Campo", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                pE.Handled = true;

            }
        }
        public void paraUsuarios(KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }


            else if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (e.KeyChar == 64 || e.KeyChar == 45 || e.KeyChar == 46 || e.KeyChar == 95 || e.KeyChar == 42 || e.KeyChar == 47)
            {
                e.Handled = false;
            }
            else

            {
                MessageBox.Show("Sólo se Aceptan Letras, Números y Sólo Éstos Símbolos: [  -_*/@.   ] En Este Campo", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                e.Handled = true;
            }
        }
        public void letrasynumeros(KeyPressEventArgs pE)
        {
            if (Char.IsLetter(pE.KeyChar))
            {
                pE.Handled = false;
            }
            else if (Char.IsDigit(pE.KeyChar))
            {
                pE.Handled = false;
            }
            else if (Char.IsControl(pE.KeyChar))
            {
                pE.Handled = false;
            }
            else if (Char.IsSeparator(pE.KeyChar))
            {
                pE.Handled = false;
            }
            else
            {
                MessageBox.Show("Sólo se Aceptan Letras y Números En Este Campo", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                pE.Handled = true;
            }
        }
        public bool camposvacioscPersonal(string credencial, string ap, string am, string nombres, string puesto)
        {
            if (!string.IsNullOrWhiteSpace(credencial) && Convert.ToInt32(credencial) > 0)
            {
                if (!string.IsNullOrWhiteSpace(ap))
                {
                    if (!string.IsNullOrWhiteSpace(am))
                    {
                        if (!string.IsNullOrWhiteSpace(nombres))
                        {
                            if (!string.IsNullOrWhiteSpace(puesto))
                            {
                                return false;
                            }
                            else
                            {

                                MessageBox.Show("El campo Puesto no puede estar vacío", "Datos Personales Icompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return true;
                            }
                        }
                        else
                        {

                            MessageBox.Show("El campo Nombres no puede estar vacío", "Datos Personales Incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return true;
                        }
                    }
                    else
                    {

                        MessageBox.Show("El campo Apellido Materno no puede estar vacío", "Datos Personales Incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return true;
                    }
                }
                else
                {

                    MessageBox.Show("El campo Apellido Paterno no puede estar vacío", "Datos Personales Incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return true;
                }

            }
            else {
                MessageBox.Show("El campo Credencial no puede estar vacío y debe ser mayor a 0", "Datos Personales Incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;

            }

        }
        public bool camposvaciospConductor(string credencial, string ap, string am, string nombres)
        {
            if (!string.IsNullOrWhiteSpace(credencial) && Convert.ToInt32(credencial) > 0)
            {
                if (!string.IsNullOrWhiteSpace(ap))
                {
                    if (!string.IsNullOrWhiteSpace(am))
                    {
                        if (!string.IsNullOrWhiteSpace(nombres))
                        {
                            return false;
                        }
                        else
                        {

                            MessageBox.Show("El campo Nombres no puede estar vacío", "Datos Personales Incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return true;
                        }
                    }
                    else
                    {

                        MessageBox.Show("El campo Apellido Materno no puede estar vacío", "Datos Personales Incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return true;
                    }
                }
                else
                {

                    MessageBox.Show("El campo Apellido Paterno no puede estar vacío", "Datos Personales Incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return true;
                }

            }
            else
            {
                MessageBox.Show("El campo Credencial no puede estar vacío y debe ser mayor a 0", "Datos Personales Incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;

            }

        }
        public string Encriptar(string texto)
        {
            try
            {

                string key = "sistemafallos"; //llave para encriptar datos

                byte[] keyArray;

                byte[] Arreglo_a_Cifrar = UTF8Encoding.UTF8.GetBytes(texto);

                //Se utilizan las clases de encriptación MD5

                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

                hashmd5.Clear();

                //Algoritmo TripleDES
                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();

                byte[] ArrayResultado = cTransform.TransformFinalBlock(Arreglo_a_Cifrar, 0, Arreglo_a_Cifrar.Length);

                tdes.Clear();

                //se regresa el resultado en forma de una cadena
                texto = Convert.ToBase64String(ArrayResultado, 0, ArrayResultado.Length);

            }
            catch (Exception)
            {

            }
            return texto;
        }
        public string Desencriptar(string textoEncriptado)
        {
            try
            {
                string key = "sistemafallos";
                byte[] keyArray;
                byte[] Array_a_Descifrar = Convert.FromBase64String(textoEncriptado);

                //algoritmo MD5
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

                hashmd5.Clear();

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();

                byte[] resultArray = cTransform.TransformFinalBlock(Array_a_Descifrar, 0, Array_a_Descifrar.Length);

                tdes.Clear();
                textoEncriptado = UTF8Encoding.UTF8.GetString(resultArray);

            }
            catch (Exception)
            {

            }
            return textoEncriptado;
        }
        public void paraUnidades(KeyPressEventArgs pE)
        {
            if (Char.IsLetter(pE.KeyChar))
            {
                pE.Handled = false;
            }
            else if (Char.IsDigit(pE.KeyChar))
            {
                pE.Handled = false;
            }
            else if (Char.IsControl(pE.KeyChar))
            {
                pE.Handled = false;
            }
            else if (pE.KeyChar == 45)
            {
                pE.Handled = false;
            }
            else
            {

                pE.Handled = true;
                MessageBox.Show("Sólo se aceptan letras y numeros en éste campo");
            }
        }
        public bool yaExisteEmpleado(string credencial, string ap, string am, string nombres)
        {

            if (!existecredencialEmpleado(credencial))
            {
                if (!existeNombreEmpleado(ap, am, nombres))
                {
                    return false;
                }
                else
                {

                    return true;
                }
            } else
            {
                return true;
            }
        }
        public int idPersonaparaUsuario(string credencial)
        {
            MySqlCommand cm = new MySqlCommand("SELECT idPersona FROM cpersonal WHERE credencial ='" + credencial + "'", c.dbconection());
            return Convert.ToInt32(cm.ExecuteScalar());
        }
        public bool yaExisteEmpleadopConductor(string credencial, string ap, string am, string nombres)
        {
            if (!existecredencialEmpleado(credencial))
            {
                if (!existeNombreEmpleado(ap, am, nombres))
                {
                    return false;
                }
                else
                {
                    MessageBox.Show("El Empleado: " + ap + " " + am + " " + nombres + " Ya Se Encuentra Registrado En El Sistema", "Datos Duplicados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            }
            else
            {
                MessageBox.Show("La Credencial " + credencial + " esta en uso", "Datos Duplicados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
        }
        public bool existepasswordUsuario(string password)
        {
            MySqlCommand cm = new MySqlCommand("SELECT count(iddato) as cuenta FROM datosistema  WHERE password ='" + Encriptar(password) + "'", c.dbconection());
            var res = cm.ExecuteScalar();

            if (Convert.ToInt32(res) > 0)
            {
                c.dbcon.Close();
                cm = new MySqlCommand("SELECT t2.status FROM datosistema AS t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal = t2.idpersona WHERE t1.password ='" + Encriptar(password) + "'", c.dbconection());
                res = cm.ExecuteScalar();
                c.dbcon.Close();
                if (Convert.ToInt32(res) == 0)
                {
                    c.dbcon.Close();
                    return false;
                }
                else
                {
                    c.dbcon.Close();
                    MessageBox.Show("Las contraseñas Deben Tener Al Menos 8 Caracteres y Contener Al Menos Dos De Los Siguientes: Letras Mayúsculas, Letras Minúsculas y Números.", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }

            }
            else
            {
                return false;
            }
        }
        public bool existeUsuarioEmpleado(string usuario)
        {
            MySqlCommand cm = new MySqlCommand("SELECT count(iddato) as cuenta FROM datosistema WHERE usuario ='" + usuario + "'", c.dbconection());
            var res = cm.ExecuteScalar();
            c.dbcon.Close();
            if (Convert.ToInt32(res) > 0)
            {
                cm = new MySqlCommand("SELECT t2.status FROM datosistema AS t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal = t2.idpersona WHERE t1.usuario ='" + usuario + "'", c.dbconection());
                res = cm.ExecuteScalar();
                c.dbcon.Close();
                if (Convert.ToInt32(res) == 0)
                {
                    return false;
                }
                else
                {
                    MessageBox.Show("El Usuario: " + usuario + " Ya Se Encuentra Registrado En El Sistema", "Datos Duplicados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }

            }
            else
            {
                return false;
            }
        }
        public bool existeNombreEmpleado(string ap, string am, string nombres)
        {
            MySqlCommand cm = new MySqlCommand("SELECT count(credencial) as cuenta FROM cpersonal WHERE CONCAT(ApPaterno, ' ',ApMaterno,' ',nombres) = CONCAT('" + ap + "', ' ','" + am + "',' ','" + nombres + "')", c.dbconection());
            var res = cm.ExecuteScalar();
            c.dbcon.Close();
            if (Convert.ToInt32(res) > 0)
            {
                cm = new MySqlCommand("SELECT status FROM cpersonal WHERE CONCAT(ApPaterno, ' ',ApMaterno,' ',nombres) = CONCAT('" + ap + "', ' ','" + am + "',' ','" + nombres + "')", c.dbconection());
                res = cm.ExecuteScalar();
                c.dbcon.Close();
                if (Convert.ToInt32(res) == 0)
                {
                    return false;
                }
                else
                {
                    MessageBox.Show("El Empleado: " + ap + " " + am + " " + nombres + " Ya Se Encuentra Registrado En El Sistema", "Datos Duplicados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }

            }
            else
            {
                return false;
            }
        }
        public bool yaExistePuesto(string puesto)
        {
            String sql = "SELECT count(idpuesto) AS cuenta From puestos WHERE puesto COLLATE utf8_bin ='" + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(puesto) + "' and status =1";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            dr.Read();

            if (dr.GetInt32("cuenta") > 0)
            {
                c.dbcon.Close();
                MessageBox.Show("El Puesto ya Existe");
                return true;
            }
            else
            {
                c.dbcon.Close();
                return false;
            }
        }
        public bool yaExisteECO(string eco)
        {
            String sql = "SELECT count(ECO) AS cuenta From cunidades WHERE ECO ='" + eco.ToUpper() + "' and status =1";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            dr.Read();

            if (dr.GetInt32("cuenta") > 0)
            {
                c.dbcon.Close();
                MessageBox.Show("El ECO ya Existe");
                return true;
            }
            else
            {
                return false;
            }
        }
        public void paraEmpresas(KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (e.KeyChar == 46 || e.KeyChar == 38)
            {
                e.Handled = false;
            }
            else
            {
                MessageBox.Show("Sólo se aceptan letras en éste campo");
                e.Handled = true;
            }
        }
        public bool yaExisteServicio(string nombre)
        {
            String sql = "SELECT count(idservicio) AS cuenta From cservicios WHERE Nombre COLLATE utf8_bin = '" + nombre + "' and status =1";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            int res = Convert.ToInt32(cm.ExecuteScalar());
            c.dbcon.Close();

            if (res > 0)
            {
                c.dbcon.Close();
                MessageBox.Show("El Servicio ya Existe", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else
            {
                c.dbcon.Close();
                return false;
            }
        }
        public bool formularioServicio(string nombre, string desc)
        {
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                if (!string.IsNullOrWhiteSpace(desc))
                {
                    return false;
                }
                else
                {
                    MessageBox.Show("El Campo 'Descripcion de Servicio' no puede Estar Vacio", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            }
            else
            {
                MessageBox.Show("El Campo 'Nombre de Servicio' no puede Estar Vacio", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
        }
        public bool existeServicioActualizar(string nombre, string _nombre)
        {
            if (!nombre.Equals(_nombre))
            {
                return yaExisteServicio(nombre);
            }
            else
            {
                return false;
            }
        }
        public bool yaExisteFalloGral(string falloGral)
        {
            String sql = "SELECT COUNT(idFalloGral) FROM cfallosgrales WHERE nombreFalloGral= '" + falloGral + "'";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            int res = Convert.ToInt32(cm.ExecuteScalar());
            c.dbcon.Close();
            if (res > 0)
            {
                MessageBox.Show("La Clasificación de Fallo Ya se Encuentra registrada en Sistema", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else
            {
                return false;
            }

        }
        public void setFolio()
        {
            string sql = "SELECT  substring(codfallo, length(codfallo)-2 , 3)  AS cuenta From cfallosesp WHERE idfalloEsp = (SELECT MAX(idfalloEsp) FROM cfallosesp)";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            dr.Read();

            folio = (dr.GetInt32("cuenta") + 1) + "";
            while (folio.Length < 4)
            {
                folio = "0" + folio;
            }
            c.dbcon.Close();
        }
        public void setFolio(string folio)
        {
            folio = folio.Substring(folio.Length - 2, 2);
            while (folio.Length < 4)
            {
                folio = "0" + folio;
            }
            this.folio = folio;
        }
        public String codFalla(String nomfalla)
        {
            if (nomfalla.Length > 0) {
                String cod = "";
                string[] split = nomfalla.Split(new char[] { '_', '.', '-', ' ' });
                foreach (string s in split)
                {

                    if (s.Trim() != "")
                        cod = cod + s.Substring(0, 1).ToUpper();
                }

                return cod + folio;
            }
            else
            {
                return "";
            }

        }
        public void paraNombreFallo(KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }


            else if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (e.KeyChar == 45 || e.KeyChar == 46 || e.KeyChar == 95 || e.KeyChar == 32)
            {
                e.Handled = false;
            }
            else
            {
                MessageBox.Show("El campo sólo acepta Letras [a-z][A-Z], numeros [1-9] y caracteres como -._");
                e.Handled = true;
            }

        }
        //public string CrearPassword()
        //{
        //    int longitud = 8;
        //    string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_-/*@";
        //    StringBuilder res = new StringBuilder();
        //    Random rnd = new Random();
        //    while (0 < longitud--)
        //    {
        //        res.Append(caracteres[rnd.Next(caracteres.Length)]);
        //    }
        //    return res.ToString();
        //}
        public bool yaExisteFalloEsp(string iddesfallo, string fallo)
        {
            String sql = "SELECT COUNT(idfalloEsp) FROM cfallosesp WHERE descfallofkcdescfallo='" + iddesfallo + "' and falloesp= '" + fallo + "'";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            int res = Convert.ToInt32(cm.ExecuteScalar());
            c.dbcon.Close();
            if (res > 0)
            {
                MessageBox.Show("El Nombre de Fallo Ya se Encuentra registrado en Sistema", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool yaExisteEmpresa(String claveEmpresa, String nombreEmpresa)
        {
            if (!existeClaveEmpresa(claveEmpresa))
            {
                if (!existenombreEmpresa(nombreEmpresa))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        public bool existeClaveEmpresa(string clave)
        {
            string sql = "SELECT count(idempresa) AS cuenta From cempresas WHERE claveEmpresa COLLATE utf8_bin = '" + clave + "'";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            dr.Read();

            if (dr.GetInt32("cuenta") > 0)
            {
                MessageBox.Show("La Clave de la Empresa Ya Ha Sido Registrada Anteriormente. Intente de Nuevo", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                c.dbcon.Close();
                return true;
            }
            else
            {
                c.dbcon.Close();
                return false;
            }
        }
        public bool existenombreEmpresa(string nombre)
        {
            string sql = "SELECT count(idempresa) AS cuenta From cempresas WHERE nombreEmpresa COLLATE utf8_bin = '" + nombre + "'";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            int res = Convert.ToInt32(cm.ExecuteScalar());
            c.dbcon.Close();
            if (res > 0)
            {
                MessageBox.Show("La Clave de la Empresa Ya Ha Sido Registrada Anteriormente. Intente de Nuevo", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool existeEmpresaActualizar(string clave, string _clave, string nombre, string _nombre)
        {
            bool res = false;
            if (!clave.Equals(_clave))
            {
                res = existeClaveEmpresa(clave);

            }
            if (!nombre.Equals(_nombre))
            {
                res = existenombreEmpresa(nombre);
            }

            return res;

        }
        public String getAreaString(int area)
        {
            String areaString = "";
            switch (area) {
                case 1:
                    areaString = "Transmasivo";
                    break;
                case 2:
                    areaString = "TRI";
                    break;
                default:
                    areaString = "";
                    break;
            }
            return areaString;
        }
        public int getIntFrombool(bool i)
        {
            if (i)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public bool getBoolFromInt(int i)
        {
            return i == 1;
        }
        public bool usuarioDesactivado(String usu)
        {

            string sql = "SELECT t2.status as cuenta FROM datosistema as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal =t2.idpersona WHERE t1.usuario ='" + usu + "'";

            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            if (dr.Read()) {
                if (dr.GetInt32("cuenta") == 0)
                {
                    c.dbcon.Close();
                    MessageBox.Show("Usuario Desactivado");
                    return true;
                }
                else
                {
                    c.dbcon.Close();
                    return false;
                }
            }
            else
            {
                c.dbcon.Close();
                return false;
            }
        }
        public bool existecredencialEmpleado(string credencial)
        {
            MySqlCommand cm = new MySqlCommand("SELECT count(credencial) as cuenta FROM cpersonal WHERE credencial = '" + credencial + "'", c.dbconection());
            var res = cm.ExecuteScalar();
            if (Convert.ToInt32(res) > 0)
            {
                cm = new MySqlCommand("SELECT status FROM cpersonal WHERE credencial = '" + credencial + "'", c.dbconection());
                res = cm.ExecuteScalar();
                c.dbcon.Close();
                if (Convert.ToInt32(res) == 0)
                {
                    return false;
                } else
                {
                    MessageBox.Show("La Credencial " + credencial + " esta en uso", "Datos Duplicados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }

            } else
            {
                return false;
            }
        }
        public bool yaExisteActualizar(string credencial, string credencialAnterior, string ap, string apAnterior, string am, string amAnterior, string nombre, string nombreAnterior, string puesto, string usu, string usuAnterior, string pass, string passAnterior) {
            bool res = false;
            if (!credencial.Equals(credencialAnterior))
            {
                res = existecredencialEmpleado(credencial);
            }
            else
            {

                if (!(ap + " " + am + " " + nombre).Equals((apAnterior + " " + amAnterior + " " + nombreAnterior)))
                {
                    res = existeNombreEmpleado(ap, am, nombre);
                }
                else
                {
                    if (!puesto.Equals("Conductor"))
                    {

                        if (!usu.Equals(usuAnterior))
                        {
                            res = existeUsuarioEmpleado(usu);
                        }
                        else
                        {
                            res = false;
                        }
                        if (!pass.Equals(passAnterior))
                        {
                            res = existepasswordUsuario(pass);
                        }
                        else
                        {
                            res = false;
                        }
                    }

                }
            }
            return res;
        }
        public bool existePasillo(string pasillo)
        {
            String sql = "SELECT count(idpasillo) AS cuenta From cpasillos WHERE pasillo ='" + pasillo + "'";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            int res = Convert.ToInt32(cm.ExecuteScalar());
            c.dbcon.Close();
            if (res > 0)
            {
                MessageBox.Show("El pasillo Ya se Encuentra registrado en Sistema", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool existeAnaquel(string pasillo, string anaquel)
        {
            String sql = "SELECT COUNT(idanaquel) FROM canaqueles as t1 WHERE t1.anaquel= '" + anaquel + "' and t1.pasillofkcpasillos='" + pasillo + "'";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            int res = Convert.ToInt32(cm.ExecuteScalar());
            c.dbcon.Close();
            if (res > 0)
            {
                MessageBox.Show("El Aanaquel Ya se Encuentra registrado en Sistema", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            else
            {
                return false;
            }
        }
        public string getStatusString(int i)
        {
            if (i == 0)
            {
                return "No Activo";
            } else
            {
                return "Activo";
            }
        }
        public int getStatusInt(string i)
        {
            if (i == "No Activo" || i == "Inactivo")
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        public bool EliminarPrivilegios(int idUsuario)
        {
            String sql = "SELECT COUNT(idprivilegio) FROM privilegios WHERE usuariofkcpersonal='" + idUsuario + "'";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            int res = Convert.ToInt32(cm.ExecuteScalar());
            c.dbcon.Close();
            if (res > 0)
            {
                if (c.insertar("DELETE FROM privilegios WHERE usuariofkcpersonal ='" + idUsuario + "'"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            } else
            {
                return true;
            }

        }
        public bool yaExisteCredencialReactivar(string credencial)
        {
            MySqlCommand cm = new MySqlCommand("SELECT count(credencial) as cuenta FROM cpersonal WHERE credencial = '" + credencial + "' and status=1", c.dbconection());
            var res = cm.ExecuteScalar();
            c.dbcon.Close();
            if (Convert.ToInt32(res) > 0)
            {
                MessageBox.Show("La Credencial " + credencial + " esta en uso", "Datos Duplicados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;

            }
            else
            {
                return false;
            }

        }
        public bool existeCharola(string anaquel, string charola)
        {
            String sql = "SELECT COUNT(idcharola) FROM ccharolas as t1 WHERE t1.charola= '" + charola + "' and t1.anaquelfkcanaqueles='" + anaquel + "'";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            int res = Convert.ToInt32(cm.ExecuteScalar());
            c.dbcon.Close();
            if (res > 0)
            {
                MessageBox.Show("La Charola Ya se Encuentra registrada en Sistema", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool existeFamilia(string nombre, string descripcion)
        {
            var res = existeNombreFamilia(nombre);
            res = existeDescFamilia(descripcion);

            return res;
        }
        public bool existeNombreFamilia(string nombre)
        {
            String sql = "SELECT COUNT(idfamilia) FROM cfamilias as t1 WHERE t1.familia= '" + nombre + "'";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            int res = Convert.ToInt32(cm.ExecuteScalar());
            c.dbcon.Close();
            if (res > 0)
            {
                MessageBox.Show("El Nombre de La Familia Ya se Encuentra registrado en Sistema", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool existeDescFamilia(string desc)
        {
            String sql = "SELECT COUNT(idfamilia) FROM cfamilias as t1 WHERE descripcionFamilia='" + desc + "'";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            int res = Convert.ToInt32(cm.ExecuteScalar());
            c.dbcon.Close();
            if (res > 0)
            {
                MessageBox.Show("La Descripción de la Familia Ya se Encuentra registrado en Sistema", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool existeUM(string um, string simbolo)
        {
            if (!existeNombreUM(um))
            {
                if (!existesimboloUM(simbolo))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        public bool existeUMActualizar(string um, string _um, string simbolo, string _simbolo)
        {
            bool res = false;
            if (!um.Equals(_um))
            {
                res = existeNombreUM(um);
            }
            if (!simbolo.Equals(_simbolo))
            {
                res = existesimboloUM(simbolo);
            }
            return res;

        }
        public bool existeNombreUM(string nombre)
        {
            string sql = "SELECT COUNT(idunidadmedida) FROM cunidadmedida as t1 WHERE t1.Nombre= '" + nombre + "'";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            int res = Convert.ToInt32(cm.ExecuteScalar());
            c.dbcon.Close();
            if (res > 0)
            {
                MessageBox.Show("El Nombre de la Unidad de Medida Ya se Encuentra registrado en Sistema", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool existesimboloUM(string simbolo)
        {
            string sql = "SELECT COUNT(idunidadmedida) FROM cunidadmedida as t1 WHERE t1.Simbolo= '" + simbolo + "'";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            int res = Convert.ToInt32(cm.ExecuteScalar());
            c.dbcon.Close();
            if (res > 0)
            {
                MessageBox.Show("El Símbolo Ya se Encuentran registrado en Sistema", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool existeMarca(string marca)
        {
            String sql = "SELECT COUNT(idmarca) FROM cmarcas as t1 WHERE t1.marca= '" + marca + "'";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            int res = Convert.ToInt32(cm.ExecuteScalar());
            c.dbcon.Close();
            if (res > 0)
            {
                MessageBox.Show("La Marca Ya se Encuentra registrada en Sistema", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            else
            {
                return false;
            }
        }
        public void paracodrefaccion(KeyPressEventArgs pE)
        {
            if (Char.IsLetter(pE.KeyChar))
            {
                pE.Handled = false;
            }
            else if (Char.IsDigit(pE.KeyChar))
            {
                pE.Handled = false;
            }
            else if (Char.IsControl(pE.KeyChar))
            {
                pE.Handled = false;
            }
            else if (Char.IsSeparator(pE.KeyChar))
            {
                pE.Handled = false;
            } else if (pE.KeyChar == 45)
            {
                pE.Handled = false;
            }
            else
            {
                MessageBox.Show("Sólo se Aceptan Letras, Números y Guiones En Este Campo", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                pE.Handled = true;
            }
        }
        public bool existeRefaccion(string cod, string nom, string modelo)
        {
            if (!existeCodigoRefaccion(cod))
            {
                if (!existeNombreRefaccion(nom))
                {
                    if (!existeModeloRefaccion(modelo))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                } else
                {
                    return true;
                }
            } else
            {
                return true;
            }
        }
        public bool existeCodigoRefaccion(string cod)
        {
            String sql = "SELECT COUNT(idrefaccion) FROM crefacciones as t1 WHERE t1.codrefaccion= '" + cod + "'";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            int res = Convert.ToInt32(cm.ExecuteScalar());
            c.dbcon.Close();
            if (res > 0)
            {
                MessageBox.Show("El Código de la Refacción Ya se Encuentra registrada en Sistema", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool existeNombreRefaccion(string nom)
        {
            String sql = "SELECT COUNT(idrefaccion) FROM crefacciones as t1 WHERE t1.nombreRefaccion= '" + nom + "'";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            int res = Convert.ToInt32(cm.ExecuteScalar());
            c.dbcon.Close();
            if (res > 0)
            {
                MessageBox.Show("El Nombre de la Refacción Ya se Encuentra registrada en Sistema", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool existeModeloRefaccion(string mod)
        {
            String sql = "SELECT COUNT(idrefaccion) FROM crefacciones as t1 WHERE t1.modeloRefaccion= '" + mod + "'";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            int res = Convert.ToInt32(cm.ExecuteScalar());
            c.dbcon.Close();
            if (res > 0)
            {
                MessageBox.Show("El Modelo de la  Refacción Ya se Encuentra registrada en Sistema", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool formularioUbicaciones(string pasillo, string anaquel, string charola, int nivel)
        {
            if (nivel == 0) {
                if (!string.IsNullOrWhiteSpace(pasillo))
                {
                    if (!string.IsNullOrWhiteSpace(anaquel))
                    {
                        if (!string.IsNullOrWhiteSpace(charola))
                        {
                            return false;
                        } else
                        {
                            MessageBox.Show("El Campo Charola no puede estár vacío", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return true;

                        }
                    } else
                    {
                        MessageBox.Show("El Campo Anaquel no puede estár vacío", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                }
                else
                {
                    MessageBox.Show("El Campo Pasillo no puede estár vacío", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            } else if (nivel == 1)
            {
                if (!string.IsNullOrWhiteSpace(anaquel))
                {
                    if (!string.IsNullOrWhiteSpace(charola))
                    {
                        return false;
                    }
                    else
                    {
                        MessageBox.Show("El Campo Charola no puede estár vacío", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                }
                else
                {
                    MessageBox.Show("El Campo Anaquel no puede estár vacío", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            } else
            {
                if (!string.IsNullOrWhiteSpace(charola))
                {
                    return false;
                }
                else
                {
                    MessageBox.Show("El Campo Charola no puede estár vacío", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            }
        }
        public bool formularioums(string nombre, string simbolo)
        {
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                if (!string.IsNullOrWhiteSpace(simbolo))
                {
                    return false;
                }
                else
                {


                    MessageBox.Show("El Campo Simbolo no puede estár vacío", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            }
            else
            {


                MessageBox.Show("El Campo Nombre no puede estár vacío", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
        }
        public string getTipoPuestoFromInt(int i) {
            if (i == 1) {
                return "De Sistema";
            } else {
                return "Complementario";
            }
        }
        public bool getTipodePuesto(int idpuesto)
        {
            string sql = "SELECT tipopuesto as tipo FROM puestos WHERE idpuesto= " + idpuesto;
            MySqlCommand cmd = new MySqlCommand(sql, c.dbconection());
            var res = cmd.ExecuteScalar();
            if (Convert.ToInt32(res) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool formularioRefaciones(string codrefaccion, string nombrerefaccion, string modelorefaccion, int familia, int um, int marca, int nivel, int pasillo, int anaquel, int charola, DateTime fecha)
        {
            if (!string.IsNullOrWhiteSpace(codrefaccion))
            {
                if (!string.IsNullOrWhiteSpace(nombrerefaccion))
                {
                    if (!string.IsNullOrWhiteSpace(modelorefaccion))
                    {
                        if (familia > 0)
                        {
                            if (um > 0)
                            {
                                if (marca > 0)
                                {
                                    if (nivel > 0)
                                    {
                                        if (pasillo > 0)
                                        {
                                            if (anaquel > 0)
                                            {
                                                if (charola > 0)
                                                {
                                                    if (fecha > DateTime.Now)
                                                    {
                                                        return false;
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("La Fecha debe ser Mayor a Hoy (" + DateTime.Now.ToLongDateString() + ")", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                        return true;
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Seleccione una Charola de La Lista Desplegable", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    return true;
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("Seleccione un Anaquel de La Lista Desplegable", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                return true;
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Seleccione un Pasillo de La Lista Desplegable", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return true;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Seleccione un Nivel de La Lista Desplegable", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return true;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Seleccione una Marca de La Lista Desplegable", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return true;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Seleccione una Unidad de Medida de La Lista Desplegable", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Seleccione una Familia de La Lista Desplegable", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("El Campo Modelo de Refacción No Puede Estar Vacío", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                }
                else
                {
                    MessageBox.Show("El Campo Nombre de Refacción No Puede Estar Vacío", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            }
            else
            {
                MessageBox.Show("El Campo Código de Refacción No Puede Estar Vacío", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
        }
        public bool NumericsUpDownRefaccion(int cantidad, int media, int abastecimiento)
        {
            if (abastecimiento > media)
            {
                MessageBox.Show("La Cantidad Ingresada para Notificación de Abastecimiento debe ser Menor a la Cantidad Ingresada para Notificacion de Media");
                return true;
            } else
            {
                if (abastecimiento > cantidad)
                {


                    MessageBox.Show("La Cantidad Ingresada para Notificación de Abastecimiento debe ser Menor a la Cantidad Ingresada a Almacén");
                    return true;
                }
                else
                {
                    if (media > cantidad)
                    {
                        MessageBox.Show("La Cantidad Ingresada para Notificación de Media debe Ser Menor a la Cantidad Ingresada a Almacén");
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        public bool formulariofamilias(string nombre, string descripcion)
        {
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                if (!string.IsNullOrWhiteSpace(descripcion))
                {
                    return false;
                }
                else
                {
                    MessageBox.Show("La Descripción de Familia no puede estar vacío", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            }
            else
            {
                MessageBox.Show("El Nombre de Familia no puede estar vacío", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
        }
        public bool validacionCorrero(string email)
        {
            try
            {
                new MailAddress(email);
                return true;
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public bool formularioFallos(string fallogral, string descfallo, string falloesp, int nivel)
        {
            if (nivel == 1)
            {
                if (!string.IsNullOrWhiteSpace(fallogral))
                {
                    if (!string.IsNullOrWhiteSpace(descfallo))
                    {
                        if (!string.IsNullOrWhiteSpace(falloesp))
                        {
                            return false;
                        }
                        else
                        {
                            MessageBox.Show("El Campo Nombre del Fallo No puede estar Vacio", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("El Campo Descripción de Fallo No puede estar Vacio", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                }
                else
                {
                    MessageBox.Show("El Campo Clasificación de Fallo No puede estar Vacio", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            } else if (nivel == 2)
            {
                if (!string.IsNullOrWhiteSpace(descfallo))
                {
                    if (!string.IsNullOrWhiteSpace(falloesp))
                    {
                        return false;
                    }
                    else
                    {
                        MessageBox.Show("El Campo Nombre del Fallo No puede estar Vacio", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                }
                else
                {
                    MessageBox.Show("El Campo Descripción de Fallo No puede estar Vacio", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            } else
            {
                if (!string.IsNullOrWhiteSpace(falloesp))
                {
                    return false;
                }
                else
                {
                    MessageBox.Show("El Campo Nombre del Fallo No puede estar Vacio", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            }
        }
        public bool yaExisteDescFallo(string descfallo)
        {
            String sql = "SELECT COUNT(iddescfallo) FROM cdescfallo WHERE descfallo= '" + descfallo + "'";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            int res = Convert.ToInt32(cm.ExecuteScalar());
            c.dbcon.Close();
            if (res > 0)
            {
                MessageBox.Show("La Descripción de Fallo Ya se Encuentra registrada en Sistema", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool formularioProveedores(string empresa, string ap, string am, string nombres, string email, string telefono, string lada)
        {
            if (!string.IsNullOrWhiteSpace(empresa))
            {
                if (!string.IsNullOrWhiteSpace(ap))
                {
                    if (!string.IsNullOrWhiteSpace(am))
                    {
                        if (!string.IsNullOrWhiteSpace(nombres))
                        {
                            if (!string.IsNullOrWhiteSpace(email))
                            {
                                if (validacionCorrero(email))
                                {
                                    if (!string.IsNullOrWhiteSpace(telefono))
                                    {
                                        if (!string.IsNullOrWhiteSpace(lada))
                                        {
                                            return false;
                                        }
                                        else
                                        {
                                            MessageBox.Show("El Campo Lada No puede Estar Vacío", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return true;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("El Campo Teléfono No puede Estar Vacío", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return true;
                                    }
                                }
                                else
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                MessageBox.Show("El Campo Correo Electronico No puede Estar Vacío", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("El Campo Empresa Nombres puede Estar Vacío", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("El Campo Apellido Materno No puede Estar Vacío", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                }
                else
                {
                    MessageBox.Show("El Campo Apellido Paterno No puede Estar Vacío", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            }
            else
            {
                MessageBox.Show("El Campo Empresa No puede Estar Vacío", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
        }
        public bool existeEmpresa(string empresa)
        {
            string sql = "SELECT COUNT(idproveedor) FROM cproveedores as t1 WHERE t1.empresa= '" + empresa + "'";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            int res = Convert.ToInt32(cm.ExecuteScalar());
            c.dbcon.Close();
            if (res > 0)
            {
                MessageBox.Show("La Empresa Ya se Encuentra registrada en Sistema", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool existeNombre(string ap, string am, string nom)
        {
            string sql = "SELECT COUNT(idproveedor) FROM cproveedores WHERE CONCAT(aPaterno,' ',aMaterno,' ',nombres) = CONCAT('" + ap + "',' ','" + am + "',' ','" + nom + "')";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            int res = Convert.ToInt32(cm.ExecuteScalar());
            c.dbcon.Close();
            if (res > 0)
            {
                MessageBox.Show("El Nombre del Proveedor Ya se Encuentra registrado en Sistema", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool existeemail(string email)
        {
            string sql = "SELECT COUNT(idproveedor) FROM cproveedores as t1 WHERE t1.correo= '" + email + "'";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            int res = Convert.ToInt32(cm.ExecuteScalar());
            c.dbcon.Close();
            if (res > 0)
            {
                MessageBox.Show("El Correo Electronico Ya se Encuentra registrado en Sistema", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool existetelefono(string telefono)
        {
            string sql = "SELECT COUNT(idproveedor) FROM cproveedores as t1 WHERE t1.telefono= '" + telefono + "'";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            int res = Convert.ToInt32(cm.ExecuteScalar());
            c.dbcon.Close();
            if (res > 0)
            {
                MessageBox.Show("El Telefono Ya se Encuentra registrado en Sistema", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool existeProveedor(string empresa, string ap, string am, string nom, string email, string telefono)
        {
            if (!existeEmpresa(empresa) && !existeNombre(ap, am, nom) && !existeemail(email) && !existetelefono(telefono))
            {
                return false;
            } else
            {
                return true;
            }
        }
        public string mayusculas(string texto)
        {
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            return ti.ToTitleCase(texto);
        }
        public bool yaExisteProveedorActualizar(string _empresa, string _empresaAnterior, string _ap, string _apAnterior, string _am, string _amAnterior, string _nombre, string _nombreAnterior, string _correo, string _correoAnterior, string _telefono, string _telefonoAnterior)
        {
            bool res = false;
            if (!_empresa.Equals(_empresaAnterior))
            {
                res = existeEmpresa(_empresa);
            }
            if (!(_ap + " " + _am + " " + _nombre).Equals(_apAnterior + " " + _amAnterior + " " + _nombreAnterior))
            {
                res = existeNombre(_ap, _am, _nombre);
            }

            if (!_correo.Equals(_correoAnterior))
            {
                res = existeemail(_correo);
                res = validacionCorrero(_correo);
            }

            if (!_telefono.Equals(_telefonoAnterior))
            {
                res = existetelefono(_telefono);
            }

            return res;
        }
        public int getStatusFromCombos(String i)
        {
            if (i == "1")
            {
                return 1;
            } else
            {
                return 0;
            }
        }
        public bool formularioUnidades(string eco, string desc)
        {
            if (!string.IsNullOrWhiteSpace(eco))
            {
                if (!string.IsNullOrWhiteSpace(desc))
                {
                    return false;
                }
                else
                {
                    MessageBox.Show("El Campo Descripcion del Económico no Puede Estar Vacío", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            }
            else
            {
                MessageBox.Show("El Campo 'ECO' no Puede Estar Vacío", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
        }
        public bool yaExisteECOActualizar(string _eco, string _ecoAnterior)
        {
            if (!_eco.Equals(_ecoAnterior))
            {
                return yaExisteECO(_eco);
            } else
            {
                return false;
            }
        }
        public bool existeFalloGralActualizar(string _fallo, string _falloAnterior)
        {
            if (!_fallo.Equals(_falloAnterior))
            {
                return yaExisteFalloGral(_fallo);
            } else
            {
                return false;
            }
        }
        public bool existeDescFalloActualizar(string _desc, string _descAnterior)
        {
            if (!_desc.Equals(_descAnterior))
            {
                return yaExisteDescFallo(_desc);
            } else
            {
                return false;
            }
        }
        public bool existenomfalloActualizar(string _ddescfallo, string _descfalloAnterior, string _nomfallo, string _nomfalloAnterior)
        {

            if (!_ddescfallo.Equals(_descfalloAnterior) || !_nomfallo.Equals(_nomfalloAnterior))
            {
                return yaExisteFalloEsp(_ddescfallo, _nomfallo);
            } else
            {
                return false;
            }

        }
        public bool formularioUnidadesTRI(string bin, string nmotor, string ntransmision, string modelo, string marca)
        {
            if (!string.IsNullOrWhiteSpace(bin))
            {
                if (!string.IsNullOrWhiteSpace(nmotor))
                {
                    if (!string.IsNullOrWhiteSpace(ntransmision))
                    {
                        if (!string.IsNullOrWhiteSpace(modelo))
                        {
                            if (!string.IsNullOrWhiteSpace(marca))
                            {
                                return false;
                            }
                            else
                            {
                                MessageBox.Show("El Campo 'Marca' No puede estar Vacio", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("El Campo 'Modelo' No puede estar Vacio", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("El Campo 'N° Serie de Transmision' No puede estar Vacio", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                }
                else
                {
                    MessageBox.Show("El Campo 'N° Serie de Motor' de Fallo No puede estar Vacio", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            }
            else
            {
                MessageBox.Show("El Campo 'VIN' No puede estar Vacio", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
        }
        public bool existeUnidadTRI(string bin, string _bin, string nmotor, string _nmotor, string transmision, string _transmision)
        {
            bool res = false;
            if (!bin.Equals(_bin))
            {
                res = existevin(bin);
            }

            if (!nmotor.Equals(_nmotor))
            {
                res = existenmotor(nmotor);
            }
            if (!transmision.Equals(_transmision))
            {
                res = existetransmision(_transmision);
            }
            return res;
        }
        public bool existevin(string bin)
        {
            string sql = "SELECT COUNT(idunidad) FROM cunidades WHERE bin= '" + bin + "'";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            int res = Convert.ToInt32(cm.ExecuteScalar());
            c.dbcon.Close();
            if (res > 0)
            {
                MessageBox.Show("El Número 'VIN' Ya se Encuentra registrado en Sistema", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool existenmotor(string nmotor)
        {
            string sql = "SELECT COUNT(idunidad) FROM cunidades WHERE nmotor= '" + nmotor + "'";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            int res = Convert.ToInt32(cm.ExecuteScalar());
            c.dbcon.Close();
            if (res > 0)
            {
                MessageBox.Show("El N° Serie de Motor Ya se Encuentra Registrado en Sistema", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool existetransmision(string transmision)
        {
            string sql = "SELECT COUNT(idunidad) FROM cunidades WHERE ntransmision = '" + transmision + "'";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            int res = Convert.ToInt32(cm.ExecuteScalar());
            c.dbcon.Close();
            if (res > 0)
            {
                MessageBox.Show("El N° Serie de Transmision Ya se Encuentra Registrado en Sistema", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool existePasilloActualizar(string pasillo, string _pasillo)
        {
            if (!pasillo.Equals(_pasillo))
            {
                return existePasillo(pasillo);
            } else
            {
                return false;
            }
        }
        public bool existeAnaquelActualizar(string pasillo, string anaquel, string _anaquel)
        {
            if (!anaquel.Equals(_anaquel))
            {
                return existeAnaquel(pasillo, anaquel);
            }
            else
            {
                return false;
            }
        }
        public bool existeCharolaActualizar(string anaquel, string charola, string _charola)
        {
            if (!charola.Equals(_charola))
            {
                return existeCharola(anaquel, charola);
            } else
            {
                return false;
            }
        }
        public bool existefamiliaActualizar(string familia, string _familia, string descripcion, string _desc) {
            var res = false;
            if (!familia.Equals(_familia))
            {
                res = existeNombreFamilia(familia);
            }
            if (!descripcion.Equals(_desc)) {
                res = existeDescFamilia(descripcion);
            }
            return res;
        }
        public bool existeMarcaActualizar(string marca, string _marca)
        {
            if (!marca.Equals(_marca))
            {
                return existeMarca(marca);
            } else
            {
                return false;
            }
        }
        public string getExistenciasFromIDRefaccion(string idrefaccion)
        {
            string sql = "SELECT CONCAT(t1.existencias,' ',t2.Simbolo) FROM crefacciones as t1 INNER JOIN cunidadmedida as t2 ON t1.umfkcunidadmedida= t2.idunidadmedida WHERE idrefaccion='" + idrefaccion + "'";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            var exist = cm.ExecuteScalar();
            return (string)exist;
        }
        public string getNivelFromID(int id)
        {
            string res = "";
            switch (id)
            {
                case 1:
                    res = "Superior";
                    break;
                case 2:
                    res = "Inferior";
                    break;
                case 3:
                    res = "Izquierdo";
                    break;
                case 4:
                    res = "Derecho";
                    break;
            }
            return res;
        }
        public bool existeRefaccionActualizar(string codref, string _codref, string nomref, string _nomref, string modref, string _modref)
        {
            bool res = false;
            if (!codref.Equals(_codref))
            {
                res = existeCodigoRefaccion(codref);
            }
            if (!nomref.Equals(_nomref))
            {
                res = existeNombreRefaccion(nomref);
            }
            if (!modref.Equals(_modref))
            {
                res = existeModeloRefaccion(modref);
            }
            return res;
        }
        public bool formularioEmpresa(string clave, string nombre)
        {
            if (!string.IsNullOrWhiteSpace(clave))
            {
                if (!string.IsNullOrWhiteSpace(nombre))
                {
                    return false;
                }
                else
                {
                    MessageBox.Show("El Campo 'Nombre de Empresa' No Puede Estar Vacío", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            }
            else
            {
                MessageBox.Show("El Campo 'Clave de Empresa' No Puede Estar Vacío", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
        }
        public bool existePuestoActualizar(string puesto, string _puesto)
        {
            if (!puesto.Equals(_puesto))
            {
                return yaExistePuesto(puesto);
            } else
            {
                return false;
            }
        }
        public bool getAccesoSistemaInt(int i)
        {
            return i == 0;
        }
        public bool formulariousu(string usu, string pass1, string pass2)
        {
            if (!string.IsNullOrWhiteSpace(usu))
            {
                if (!string.IsNullOrWhiteSpace(pass1))
                {
                    if (!string.IsNullOrWhiteSpace(pass2))
                    {
                        if (pass1.Length >= 8)
                        {
                            if (pass1.Equals(pass2))
                            {
                                return false;
                            }
                            else
                            {
                                MessageBox.Show("Las Contraseñas No Coinciden", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Las contraseñas Deben Tener Al Menos 8 Caracteres y Contener Al Menos Dos De Los Siguientes: Letras Mayúsculas, Letras Minúsculas y Números.", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("El Campo 'Confirmar Contraseña' No Puede Estar Vacío", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                }
                else
                {
                    MessageBox.Show("El Campo 'Contraseña' No Puede Estar Vacío", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            }
            else
            {
                MessageBox.Show("El Campo 'Usuario' No Puede Estar Vacío", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
        }
        public bool existeusupass(string usu, string pass)
        {
            if (!existeUsuarioEmpleado(usu))
            {
                if (!existepasswordUsuario(pass))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        public bool codrefaccionValido(string valido)
        {
            string palabraValida = @"\A[A-Z]+-?\b[A-Z]*\d+[A-Z]*\Z";
            if (Regex.IsMatch(valido, palabraValida))
            {
                return false;
            } else
            {
                MessageBox.Show("El Código de Refacción Tecleado No es Válido.", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
        }
        public bool palabraValida(string valido)
        {
                string palabraValida = @"[A-Z]+\s*\d*";
            if (Regex.IsMatch(valido, palabraValida))
            {
                return true;
            }
            else
            {
              return false;
            }
        }

    }
    }

