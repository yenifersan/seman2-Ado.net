using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace semana2
{
    public partial class FrmPedidosDetalle : Form
    {
        public FrmPedidosDetalle()
        {
            InitializeComponent();
        }
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Leon"].ConnectionString);

        public void ListaAnios()
        {
            using (SqlCommand cmd = new SqlCommand("Usp_ListaAnios", cn))
            {
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = cmd;
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    using (DataSet df = new DataSet())
                    {
                        //el metodo fill carga los datos 
                        da.Fill(df, "ListaAnios");
                        //enviar
                        CboAnios2.DataSource = df.Tables["ListaAnios"];
                        CboAnios2.DisplayMember = "Anios";
                        CboAnios2.ValueMember = "Anios";
                    }

                }
            }
        }

        private void FrmPedidosDetalle_Load(object sender, EventArgs e)
        {
        
            ListaAnios();
        }


        private void CboAnios_SelectionChangeCommitted(object sender, EventArgs e)
        {

            using (SqlCommand cmd = new SqlCommand("Usp_Lista_Pedidos_Anios", cn))
            {
                using (SqlDataAdapter Da = new SqlDataAdapter())
                {
                    Da.SelectCommand = cmd;
                    Da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    Da.SelectCommand.Parameters.AddWithValue("@anio", CboAnios2.SelectedValue);
                    using (DataSet df = new DataSet())
                    {
                        Da.Fill(df,"Pedidos");
                        //mostrar
                        DgPedidos.DataSource = df.Tables["Pedidos"];
                        lblNumero.Text = df.Tables["Pedidos"].Rows.Count.ToString();
                    }
                }
            }
        }


        private void DgPedidos_DoubleClick(object sender, EventArgs e)
        {
            //capturar
            int Codigo;
            Codigo = Convert.ToInt32(DgPedidos.CurrentRow.Cells[0].Value);
            using (SqlCommand cmd = new SqlCommand("Usp_Detalle_Pedido", cn))
            {
                using (SqlDataAdapter Da = new SqlDataAdapter())

                {
                    Da.SelectCommand = cmd;
                    Da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    Da.SelectCommand.Parameters.AddWithValue("@idpedido", Codigo);
                    using (DataSet df = new DataSet())
                    {
                        Da.Fill(df, "detallespedidos");
                        //mostrar
                        DgDetalle.DataSource = df.Tables["detallespedidos"];
                        LblMonto.Text = df.Tables["detallespedidos"].Compute("Sum(Monto)", "").ToString();
                    }
                }


            }

        }
    }
}
    

