using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;//-->
using System.Data.SqlClient;


namespace Döviz_Ofisi_Projesi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //https://www.tcmb.gov.tr/kurlar/today.xml bu siteden cektik
        SqlConnection baglantı = new SqlConnection("Data Source=MONSTER;Initial Catalog=Merkez_bankası;Integrated Security=True");
        private void Form1_Load(object sender, EventArgs e)
        {
            string bugun = "https://www.tcmb.gov.tr/kurlar/today.xml";//merkez bankası kur sitesin
            var xmldosya = new XmlDocument();//var(her değişkeni tutuyordu) xmldosya adında xmldoxument olusturduk
            xmldosya.Load(bugun);//xmldosyayı loadımıza bugun adlı değişkende yazan adresi yukle

            string dolaralıs = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteBuying").InnerXml;
     //dolaralıs adında degişken olusturudk secilen tek deger(             //siteyi acıp bak anlarsın(dolar alus)
            lbl_DOLAR_ALIŞ.Text = dolaralıs;

            string dolarsatıs = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteSelling").InnerXml;
            lbl_DOLAR_SATIS.Text = dolarsatıs;

            string euroalıs = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteBuying").InnerXml;
            //secilen tek deger(
            lbl_EURO_ALIŞ.Text = euroalıs;

            string eurosatıs = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteSelling").InnerXml;
            lbl_EURO_SATIS.Text = eurosatıs;

            baglantı.Open();
            SqlCommand komut = new SqlCommand("select * from Table_1", baglantı);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            baglantı.Close();

        }

        private void btn_Dolar_al_Click(object sender, EventArgs e)
        {    //textboxa kuru yollama
             txt_KUR.Text = lbl_DOLAR_ALIŞ.Text;
        }

        private void btn_Dolar_Sat_Click(object sender, EventArgs e)
        {
            //textboxa kuru yollama
            txt_KUR.Text = lbl_DOLAR_SATIS.Text;
        }

        private void btn_Euro_al_Click(object sender, EventArgs e)
        {
            //textboxa kuru yollama
            txt_KUR.Text = lbl_EURO_ALIŞ.Text;
        }

        private void btn_Euro_sat_Click(object sender, EventArgs e)
        {
            //textboxa kuru yollama
            txt_KUR.Text = lbl_EURO_SATIS.Text;
        }

        private void btn_Satış_YAP_Click(object sender, EventArgs e)
        {   //Tutar Hesaplama

            // Tutar Hesaplama
            double kur, miktar, tutar;
            kur = Convert.ToDouble(txt_KUR.Text);
            miktar = Convert.ToDouble(txt_MİKTAR.Text);
            tutar = kur * miktar;
            txt_TUTAR.Text = tutar.ToString();


            baglantı.Open();
            SqlCommand komut = new SqlCommand("UPDATE Table_1 SET TL = TL + @p1, USD = USD - @p2 ,EURO=EURO-@p3", baglantı);
            komut.Parameters.AddWithValue("@p1", tutar);
            komut.Parameters.AddWithValue("@p2", miktar);
            komut.Parameters.AddWithValue("@p3", miktar);
            komut.ExecuteNonQuery();
            baglantı.Close();










        }

        private void txt_KUR_TextChanged(object sender, EventArgs e)
        {   //TextChanged(Herhangibir Değişiklik olduğunda ne yapsın)
            //Nokta'yı Virgül'e Çeviriyoruz çünkü c# kendisi ceviremiyor acıklama satırı yap dene istersen
            txt_KUR.Text = txt_KUR.Text.Replace(".", ",");
                                        //replace(cevir)(.yı,e)

        }

        private void button1_Click(object sender, EventArgs e)
        {
            double kur, miktar, tutar,kalan;
            kur = Convert.ToDouble(txt_KUR.Text);//kur-miktarı texboxlardan alacak
            miktar = Convert.ToDouble(txt_MİKTAR.Text);
            tutar = Convert.ToInt16( miktar / kur);
            txt_TUTAR.Text = tutar.ToString();
            kalan = (miktar % kur);
            txt_KALAN.Text = kalan.ToString();
        }

        private void btn_Dolar_al_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
