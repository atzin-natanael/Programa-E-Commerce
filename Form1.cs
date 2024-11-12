using ClosedXML.Excel;
using FirebirdSql.Data.FirebirdClient;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using static Articulos_Ecommerce.Form1;

namespace Articulos_Ecommerce
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Config();
        }
        public void Config()
        {
            string filePath = "C:\\ConfigDB\\DB.txt"; // Ruta de tu archivo de texto
            List<string> lineas = new List<string>();

            // Verificar si el archivo existe
            if (File.Exists(filePath))
            {
                // Leer todas las líneas del archivo
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string linea;
                    while ((linea = sr.ReadLine()) != null)
                    {
                        GlobalSettings.Instance.Config.Add(linea);
                    }

                }
                GlobalSettings.Instance.Ip = GlobalSettings.Instance.Config[0];
                GlobalSettings.Instance.Puerto = GlobalSettings.Instance.Config[1];
                GlobalSettings.Instance.Direccion = GlobalSettings.Instance.Config[2];
                GlobalSettings.Instance.User = GlobalSettings.Instance.Config[3];
                GlobalSettings.Instance.Pw = GlobalSettings.Instance.Config[4];
            }

        }
        public List<Resultado> ObtenerArticulos(string DOCTO_VE_ID)
        {
            var resultados = new List<Resultado>();

            using (FbConnection con = new FbConnection("User=" + GlobalSettings.Instance.User + ";" +
                                                       "Password=" + GlobalSettings.Instance.Pw + ";" +
                                                       "Database=" + GlobalSettings.Instance.Direccion + ";" +
                                                       "DataSource=" + GlobalSettings.Instance.Ip + ";" +
                                                       "Port=" + GlobalSettings.Instance.Puerto + ";" +
                                                       "Dialect=3;" + "Charset=UTF8;"))
            {
                try
                {
                    con.Open();
                    FbCommand command = new FbCommand("ARTS_DOCTO_DIARIO_VE", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("V_DOCTO_VE_ID", FbDbType.Integer).Value = DOCTO_VE_ID;
                    //FbParameter paramDOCTO_ID = new FbParameter("DOCTO_ID", FbDbType.Integer);
                    //paramDOCTO_ID.Direction = ParameterDirection.Output;
                    //command.Parameters.Add(paramDOCTO_ID);

                    //FbParameter paramARTICULO_ID = new FbParameter("ARTICULO_ID", FbDbType.Integer);
                    //paramARTICULO_ID.Direction = ParameterDirection.Output;
                    //command.Parameters.Add(paramARTICULO_ID);

                    //FbParameter paramNOMBRE = new FbParameter("NOMBRE", FbDbType.VarChar, 100); // Ajusté el tipo a VarChar
                    //paramNOMBRE.Direction = ParameterDirection.Output;
                    //command.Parameters.Add(paramNOMBRE);

                    //FbParameter paramUNIDAD_VENTA = new FbParameter("UNIDAD_VENTA", FbDbType.VarChar, 20); // Ajusté el tipo a VarChar
                    //paramUNIDAD_VENTA.Direction = ParameterDirection.Output;
                    //command.Parameters.Add(paramUNIDAD_VENTA);

                    //FbParameter paramUNIDADES = new FbParameter("UNIDADES", FbDbType.Numeric);
                    //paramUNIDADES.Direction = ParameterDirection.Output;
                    //command.Parameters.Add(paramUNIDADES);
                    using (FbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Asegúrate de leer el índice o nombre de columna adecuado
                            string articulo_id = reader["ARTICULO_ID"] != DBNull.Value ? reader["ARTICULO_ID"].ToString() : "0";
                            string articulo = reader["NOMBRE_ART"] != DBNull.Value ? reader["NOMBRE_ART"].ToString() : "0";
                            string unidad = reader["UNIDADES"] != DBNull.Value ? reader["UNIDADES"].ToString() : "0";
                            decimal importe = reader["IMPORTE_NETO"] != DBNull.Value ? Decimal.Parse(reader["IMPORTE_NETO"].ToString()) : 0m;

                            resultados.Add(new Resultado
                            {
                                Articulo_ID = articulo_id,
                                Articulo = articulo,
                                Unidad = unidad,
                                Importe = importe
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Se perdió la conexión :( , contacta a 06 o intenta de nuevo", "¡Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show(ex.ToString());
                }
            }

            return resultados;
        }
        public class Resultado
        {
            public string Articulo_ID { get; set; }
            public string Articulo { get; set; }
            public string Unidad { get; set; }
            public decimal Importe { get; set; }
        }
        public class Costo
        {
            public string Articulo_id { get; set; }
            public string RCosto { get; set; }
        }
        public string Codigo(string articulo_id)
        {
            FbConnection con = new FbConnection("User=" + GlobalSettings.Instance.User + ";" + "Password=" + GlobalSettings.Instance.Pw + ";" + "Database=" + GlobalSettings.Instance.Direccion + ";" + "DataSource=" + GlobalSettings.Instance.Ip + ";" + "Port=" + GlobalSettings.Instance.Puerto + ";" + "Dialect=3;" + "Charset=UTF8;");
            try
            {
                con.Open();
                string query = "SELECT CLAVE_ARTICULO FROM CLAVES_ARTICULOS WHERE ARTICULO_ID = ' " + articulo_id + "' AND ROL_CLAVE_ART_ID = '17'; ";
                //string query7 = "SELECT CLIENTE_ID FROM DOCTOS_VE WHERE FECHA BETWEEN '" + DateInicio1.Value.ToString("dd.MM.yyyy") + "' AND '" + DateFin1.Value.ToString("dd.MM.yyyy") + "' ";
                FbCommand command0 = new FbCommand(query, con);
                FbDataReader reader0 = command0.ExecuteReader();
                if (reader0.Read())
                {
                    return reader0.GetString(0);
                }
                else
                {
                    return "NOT FOUND";
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show("Se perdió la conexión :( , contacta a 06 o intenta de nuevo", "¡Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(ex.ToString());
                return "NOT FOUND";
            }
            finally
            {
                con.Close();
            }
        }
        public Costo FindCosto(string docto_ve_id, string articulo_id)
        {
            FbConnection con = new FbConnection("User=" + GlobalSettings.Instance.User + ";" + "Password=" + GlobalSettings.Instance.Pw + ";" + "Database=" + GlobalSettings.Instance.Direccion + ";" + "DataSource=" + GlobalSettings.Instance.Ip + ";" + "Port=" + GlobalSettings.Instance.Puerto + ";" + "Dialect=3;" + "Charset=UTF8;");
            try
            {
                con.Open();
                FbCommand command = new FbCommand("GET_COSTO_ART_DOCTO_VE", con);
                command.CommandType = CommandType.StoredProcedure;

                // Parámetros de entrada
                command.Parameters.Add("V_DOCTO_VE_ID", FbDbType.Integer).Value = docto_ve_id;
                // Parámetro de salida


                // Parámetros de entrada
                command.Parameters.Add("V_ARTICULO_ID", FbDbType.Integer).Value = articulo_id;
                // Parámetro de salida


                FbParameter paramCOSTO = new FbParameter("COSTO_ART", FbDbType.Numeric);
                paramCOSTO.Direction = ParameterDirection.Output;
                command.Parameters.Add(paramCOSTO);

                // Ejecutar el procedimiento almacenado
                command.ExecuteNonQuery();
                string costo = paramCOSTO.Value != DBNull.Value ? paramCOSTO.Value.ToString() : "0";
                return new Costo { Articulo_id = articulo_id, RCosto = costo };
                //MessageBox.Show("ALMACÉN: "+ Existencia.ToString() +"\n TIENDA: "+ ExistenciaTienda.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se perdió la conexión :( , contacta a 06 o intenta de nuevo", "¡Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(ex.ToString());
                return new Costo { Articulo_id = articulo_id, RCosto = "0" };
            }
            finally
            {
                con.Close();
            }
        }
        public void FuncionConsulta()
        {
            FbConnection con = new FbConnection("User=" + GlobalSettings.Instance.User + ";" + "Password=" + GlobalSettings.Instance.Pw + ";" + "Database=" + GlobalSettings.Instance.Direccion + ";" + "DataSource=" + GlobalSettings.Instance.Ip + ";" + "Port=" + GlobalSettings.Instance.Puerto + ";" + "Dialect=3;" + "Charset=UTF8;");
            try
            {
                con.Open();
                // Utiliza parámetros para evitar la inyección de SQL

                string query = "SELECT DOCTO_VE_ID, FOLIO, FECHA FROM DOCTOS_VE WHERE TIPO_DOCTO = 'F' AND (FOLIO LIKE 'PM%' OR FOLIO LIKE 'PE%' OR FOLIO LIKE 'PA%') AND FECHA BETWEEN '" + Inicio.Value.ToString("yyyy-MM-dd") + "' AND '" + Fin.Value.ToString("yyyy-MM-dd") + "'; ";
                //string query7 = "SELECT CLIENTE_ID FROM DOCTOS_VE WHERE FECHA BETWEEN '" + DateInicio1.Value.ToString("dd.MM.yyyy") + "' AND '" + DateFin1.Value.ToString("dd.MM.yyyy") + "' ";
                FbCommand command0 = new FbCommand(query, con);
                FbDataReader reader0 = command0.ExecuteReader();
                List<List<string>> ResultadoQ1 = new List<List<string>>();
                //bool bandera = inicio == "Inicio" ? true : false;
                int i = 2;
                while (reader0.Read())
                {

                    List<string> list = new List<string>();
                    list.Add(reader0.GetString(0));
                    list.Add(reader0.GetString(1));
                    list.Add(reader0.GetString(2));
                    ResultadoQ1.Add(list);

                    // Guarda el archivo
                }

                using (var workbook = new XLWorkbook())
                {
                    // Añade una hoja llamada "Inventario"
                    var worksheet = workbook.Worksheets.Add("FOLIOS");
                    worksheet.Cell(1, 1).Value = "DOCTO_VE";
                    worksheet.Cell(1, 2).Value = "FOLIO";
                    worksheet.Cell(1, 3).Value = "FECHA";
                    worksheet.Cell(1, 4).Value = "ARTICULO_ID";
                    worksheet.Cell(1, 5).Value = "NOMBRE";
                    worksheet.Cell(1, 6).Value = "UNIDAD";
                    worksheet.Cell(1, 7).Value = "COSTO";
                    worksheet.Cell(1, 8).Value = "IMPORTE";

                    worksheet.Column(1).Width = 20;
                    worksheet.Column(2).Width = 20;
                    worksheet.Column(3).Width = 20;
                    worksheet.Column(4).Width = 20;
                    worksheet.Column(5).Width = 50;
                    worksheet.Column(6).Width = 20;
                    worksheet.Column(7).Width = 15;
                    worksheet.Column(8).Width = 15;
                    for (int j = 0; j < ResultadoQ1.Count; j++)
                    {
                        string fecha = ResultadoQ1[j][2].ToString();
                        string resultado = fecha.Substring(0, 10);
                        worksheet.Cell(i, 1).Value = ResultadoQ1[j][0];
                        worksheet.Cell(i, 2).Value = ResultadoQ1[j][1];
                        worksheet.Cell(i, 3).Value = resultado;
                        List<Resultado> valores = ObtenerArticulos(ResultadoQ1[j][0]);
                        int renglones = 0;
                        foreach (var resp in valores)
                        {
                            worksheet.Cell(i, 1).Value = ResultadoQ1[j][0];
                            worksheet.Cell(i, 2).Value = ResultadoQ1[j][1];
                            worksheet.Cell(i, 3).Value = resultado;
                            string codigo = Codigo(valores[renglones].Articulo_ID);
                            worksheet.Cell(i, 4).Value = codigo;
                            worksheet.Cell(i, 5).Value = valores[renglones].Articulo;
                            worksheet.Cell(i, 6).Value = valores[renglones].Unidad;
                            Costo respuesta = FindCosto(ResultadoQ1[j][0], valores[renglones].Articulo_ID);
                            worksheet.Cell(i, 7).Value = respuesta.RCosto;
                            worksheet.Cell(i, 8).Value = valores[renglones].Importe;
                            // Procesa cada artículo aquí
                            ++i;
                            renglones++;
                        }
                    }
                    //// Añade una hoja llamada "Inventario"
                    //var worksheet2 = workbook.Worksheets.Add("ARTICULOS");
                    //worksheet2.Cell(1, 1).Value = "ARTICULO_ID";
                    //worksheet2.Cell(1, 2).Value = "NOMBRE";
                    //worksheet2.Cell(1, 3).Value = "UNIDAD";
                    //worksheet2.Cell(1, 4).Value = "COSTO";

                    ////worksheet.Cell(1, 3).Value = "FECHA";
                    //worksheet2.Column(1).Width = 20;
                    //worksheet2.Column(2).Width = 20;
                    ////worksheet.Column(3).Width = 20;
                    //int x = 2;
                    //for (int j = 0; j < ResultadoQ1.Count; j++)
                    //{
                    //    Resultado valores = Articulo(ResultadoQ1[j][0]);

                    //    worksheet2.Cell(x, 1).Value = valores.Articulo_ID;
                    //    worksheet2.Cell(x, 2).Value = valores.Articulo;
                    //    worksheet2.Cell(x, 3).Value = valores.Unidad;
                    //    Costo respuesta = FindCosto(ResultadoQ1[j][0], valores.Articulo_ID);
                    //    worksheet2.Cell(x, 4).Value = respuesta.RCosto;
                    //    ++x;
                    //}
                    workbook.SaveAs(GlobalSettings.Instance.filePath);
                }
                Process.Start(new ProcessStartInfo(GlobalSettings.Instance.filePath) { UseShellExecute = true });
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Se perdió la conexión :( , contacta a 06 o intenta de nuevo", "¡Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(ex.ToString());
                return;
            }
            finally
            {
                con.Close();
            }
        }
        private void Consultar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("PRIMERO ASIGNALE UN NOMBRE AL ARCHIVO Y SU UBICACIÓN");
            string desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog1.InitialDirectory = desktopFolder;
            saveFileDialog1.Title = "ASIGNAR NOMBRE";
            saveFileDialog1.FileName = "Reporte " + DateTime.Now.ToString("dddd dd-MM-yy");
            saveFileDialog1.Filter = "Archivos de Excel (*.xlsx)|*.xlsx|Todos los archivos (*.*)|*.*";
            saveFileDialog1.DefaultExt = "xlsx";
            saveFileDialog1.AddExtension = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                GlobalSettings.Instance.filePath = saveFileDialog1.FileName;
                FuncionConsulta();
            }
            else
            {
                return;
            }
        }
    }
}