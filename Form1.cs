using System;
using newCRUD.DataSet1TableAdapters;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Globalization;

namespace newCRUD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Initialize();
            Action();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button6.Enabled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Console.WriteLine(e.ToString());
        }



        private void Initialize()
        {
            comboBox1.Items.Clear();
            listBox1.Items.Clear();
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            DataSet1TableAdapters.saArtPrecioTableAdapter ta = new DataSet1TableAdapters.saArtPrecioTableAdapter();
            DataSet1.saArtPrecioDataTable dt = null; // Declaración fuera del bloque try

            try
            {
                dt = ta.GetData(); // Inicialización dentro del bloque try
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al conectar con la base de datos." );
                Environment.Exit(1);
            }

            comboBox1.Items.Add("TODOS");

            if (dt != null)
            {
                // Crear un HashSet para almacenar tipos de precios únicos.
                HashSet<string> tiposDePreciosUnicos = new HashSet<string>();

                // Recorrer el DataTable y agregar los tipos de precios únicos al HashSet.
                foreach (DataRow row in dt.Rows)
                {
                    if (row["co_precio"] is string tipoPrecio)
                    {
                        tiposDePreciosUnicos.Add(tipoPrecio);
                    }
                }
                // Agregar los tipos de precios únicos al ComboBox.
                foreach (string tipoPrecioUnico in tiposDePreciosUnicos)
                {
                    comboBox1.Items.Add(tipoPrecioUnico);
                }

                // Crear un HashSet para almacenar fechas únicas.
                HashSet<string> fechasUnicas = new HashSet<string>();

                // Recorrer el DataTable y agregar las fechas únicas al HashSet.
                foreach (DataRow row in dt.Rows)
                {
                    if (row["fe_us_in"] is DateTime fecha)
                    {
                        // Agregar la fecha formateada como cadena para evitar duplicados
                        fechasUnicas.Add(fecha.ToString("yyyy/MM/dd"));
                    }
                }

                // Ordenar las fechas únicas de más antigua a más reciente.
                List<string> fechasOrdenadas = fechasUnicas.OrderBy(fecha => DateTime.ParseExact(fecha, "yyyy/MM/dd", null)).ToList();

                // Agregar las fechas únicas ordenadas al ListBox.
                foreach (string fechaUnica in fechasOrdenadas)
                {
                    listBox1.Items.Add(fechaUnica); // Agrega las fechas ordenadas de más antigua a más reciente
                }

                button6.Enabled = false;
                button5.Enabled = false;
                // Verifica si el ListBox contiene elementos.
                if (listBox1.Items.Count > 0)
                {
                    // Ordena los elementos del ListBox en orden descendente.
                    listBox1.Sorted = true;
                    listBox1.Sorted = false;

                    // Selecciona automáticamente el último elemento, que será la fecha más reciente.
                    listBox1.SetSelected(listBox1.Items.Count - 1, true);
                }
            }
            comboBox1.SelectedIndex = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }


        private void Action()
        {
            button5.Enabled = false;
            button6.Enabled = false;

            // Agrega filas y datos específicos para la "Opción 1"
            DataSet1TableAdapters.saArtPrecioTableAdapter ta =
            new DataSet1TableAdapters.saArtPrecioTableAdapter();
            DataSet1.saArtPrecioDataTable dt = ta.GetData();

            string opcionSeleccionada = comboBox1.SelectedItem.ToString();
            string fechaSeleccionada = listBox1.SelectedItem.ToString();


            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();

            DateTime fechaFiltro = DateTime.Now.Date;


            // Llena el DataGridView según la opción seleccionada
            if (opcionSeleccionada == "TODOS")
            {
                var datosFiltrados = dt.Where(item => item.fe_us_in.ToString("yyyy/MM/dd") == fechaSeleccionada).ToList();
                dataGridView1.DataSource = dt;
            }
            else
            {
                var datosFiltrados = dt.Where(item => item.co_precio == opcionSeleccionada && item.fe_us_in.ToString("yyyy/MM/dd") == fechaSeleccionada).ToList();
                dataGridView1.DataSource = datosFiltrados;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Add("1", "CODIGO_PRODUCTO");
            dataGridView1.Columns.Add("2", "TIPO_PRECIO");
            dataGridView1.Columns.Add("3", "PRECIO");
            button5.Enabled = true;
            button6.Enabled = false;
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void InsertarDatosDesdeDataGridView()
        {
            // Cadena de conexión a tu base de datos SQL Server.
            string connectionString = "Data Source=25.55.146.112\\SQL2019;Initial Catalog=INVMMB_P;User ID=profit;Password=profit;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Supongamos que 'dataGridView1' es tu DataGridView.
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            // Verifica si la fila no es la fila de encabezado y contiene datos válidos.
                            if (!row.IsNewRow && row.Cells[0].Value != null)
                            {
                                string co_art = row.Cells[0].Value.ToString();
                                string co_precio = row.Cells[1].Value.ToString();
                                string co_alma_calculado = "TODOS";
                                DateTime desde = DateTime.Now;
                                string hasta = null;
                                string co_alma = null;
                                string monto = row.Cells[2].Value.ToString();
                                string montoadi1 = null;
                                string montoadi2 = null;
                                string montoadi3 = null;
                                string montoadi4 = null;
                                string montoadi5 = null;
                                string precioOm = "1";
                                string co_us_in = "MAFA";
                                string co_sucu_in = "02";
                                DateTime fe_us_in = DateTime.Now;
                                string co_us_mo = "999";
                                string co_sucu_mo = "01";
                                DateTime fe_us_mo = DateTime.Now;
                                string revisado = null;
                                string transfe = null;
                                string validador = "";
                                string co_mone = "USD";
                                bool Inactivo = false;                                                // Continúa para otras columnas según sea necesario.

                                // Consulta SQL para la inserción.
                                string query = "INSERT INTO saArtPrecio (co_art, co_precio, desde, monto, precioOm, co_us_in, co_sucu_in, fe_us_in, co_us_mo, co_sucu_mo, fe_us_mo, co_mone, Inactivo) VALUES (@co_art, @co_precio, @desde, @monto, @precioOm, @co_us_in, @co_sucu_in, @fe_us_in, @co_us_mo, @co_sucu_mo, @fe_us_mo, @co_mone, @Inactivo)";
                                using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@co_art", co_art);
                                    cmd.Parameters.AddWithValue("@co_precio", co_precio);
                                    cmd.Parameters.AddWithValue("@desde", desde);
                                    cmd.Parameters.AddWithValue("@monto", monto);
                                    cmd.Parameters.AddWithValue("@precioOm", precioOm);
                                    cmd.Parameters.AddWithValue("@co_us_in", co_us_in);
                                    cmd.Parameters.AddWithValue("@co_sucu_in", co_sucu_in);
                                    cmd.Parameters.AddWithValue("@fe_us_in", fe_us_in);
                                    cmd.Parameters.AddWithValue("@co_us_mo", co_us_mo);
                                    cmd.Parameters.AddWithValue("@co_sucu_mo", co_sucu_mo);
                                    cmd.Parameters.AddWithValue("@fe_us_mo", fe_us_mo);
                                    cmd.Parameters.AddWithValue("@co_mone", co_mone);
                                    cmd.Parameters.AddWithValue("@Inactivo", Inactivo);


                                    // Añade parámetros para otras columnas si es necesario.

                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }

                        transaction.Commit();
                        MessageBox.Show("Datos insertados correctamente.");
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            transaction.Rollback();


                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Error al insertar datos: " + ex.Message);

                        }
                    }
                }
            }
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count > 0 && comboBox1.SelectedItem != null)
            {
                Action();
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count > 0 && comboBox1.SelectedItem != null)
            {
                Action();
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            // Verifica si se ha presionado Ctrl+V (Ctrl y la tecla V al mismo tiempo).
            if (e.Control && e.KeyCode == Keys.V)
            {
                // Obtén el contenido del portapapeles.
                string clipboardText = Clipboard.GetText();

                // Divide el texto en líneas y luego en celdas separadas por tabulaciones.
                string[] lines = clipboardText.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in lines)
                {
                    string[] cells = line.Split('\t');

                    // Agrega una nueva fila y asigna los valores de las celdas.
                    int rowIndex = dataGridView1.Rows.Add();
                    for (int i = 0; i < cells.Length; i++)
                    {
                        dataGridView1.Rows[rowIndex].Cells[i].Value = cells[i];
                    }
                }

                // Indica que el evento ha sido manejado.
                e.Handled = true;
            }
        }

        private void EliminarDatosDesdeDataGridView()
        {
            // Cadena de conexión a tu base de datos SQL Server.
            string connectionString = "Data Source=25.55.146.112\\SQL2019;Initial Catalog=INVMMB_P;User ID=profit;Password=profit;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string fechaSeleccionada = listBox1.SelectedItem.ToString();


                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Supongamos que quieres eliminar registros donde la columna 'fe_us_in' cumple cierta condición.
                        DateTime fecha = DateTime.ParseExact(fechaSeleccionada, "yyyy/MM/dd", CultureInfo.InvariantCulture); ; // Establece la fecha que determina qué registros eliminar.

                        // Consulta SQL para la eliminación de registros.
                        string query = "DELETE FROM saArtPrecio WHERE CONVERT(DATE, fe_us_in) = @fecha";
                        using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@fecha", fecha.ToString("yyyy/MM/dd")); // Asigna el valor de la fecha a eliminar.

                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        MessageBox.Show("Datos eliminados correctamente. (" + fecha.ToString("yyyy/MM/dd") + ")");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error al eliminar datos: " + ex.Message);
                    }
                }
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            // Muestra un cuadro de diálogo de confirmación.
            DialogResult resultado = MessageBox.Show("¿Estás seguro de ejecutar esta acción?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Comprueba la respuesta del usuario.
            if (resultado == DialogResult.Yes)
            {
                InsertarDatosDesdeDataGridView();
                Initialize();
                Action();
            }
            else
            {
                // El usuario ha cancelado la acción o ha elegido "No".
                // Puedes colocar aquí el código para manejar la cancelación.
            }
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            // Muestra un cuadro de diálogo de confirmación.
            DialogResult resultado = MessageBox.Show("¿Estás seguro de ejecutar esta acción?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Comprueba la respuesta del usuario.
            if (resultado == DialogResult.Yes)
            {
                button5.Enabled = false;
                EliminarDatosDesdeDataGridView();
                Initialize();
                Action();
            }
            else
            {
                // El usuario ha cancelado la acción o ha elegido "No".
                // Puedes colocar aquí el código para manejar la cancelación.
            }

        }
    }
}