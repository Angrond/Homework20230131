using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace nevgenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<string> csaladiNevek = new ObservableCollection<string>();
        ObservableCollection<string> csaladiNevek2 = new ObservableCollection<string>();
        ObservableCollection<string> utoNevek=new ObservableCollection<string>();
        ObservableCollection<string> utoNevek2 = new ObservableCollection<string>();

        Random vel;
        public MainWindow()
        {
            InitializeComponent();
            vel = new Random();

        }

        private void btnBetoltCsaladneveket_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog()==true)
            {
                foreach (var nev in File.ReadAllLines(ofd.FileName))
                {
                    csaladiNevek.Add(nev);
                }

                lbCsaladnevek.ItemsSource = csaladiNevek;
            }
            
            
        }

        private void btnBetoltUtoneveket_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd2 = new OpenFileDialog();
            {
                if (ofd2.ShowDialog() == true)
                {
                    foreach (var nev in File.ReadAllLines(ofd2.FileName))
                    {
                        utoNevek.Add(nev);
                    }
                }

                lbUtonevek.ItemsSource = utoNevek;
            }
        }

        private void btnGeneral_Click(object sender, RoutedEventArgs e)
        {
            if (lbCsaladnevek.Items.Count == 0 || lbUtonevek.Items.Count==0)
            {
                MessageBox.Show("Nincs név a listában.");
            }
            lbRendez.Visibility = Visibility.Hidden;
            if (rbEgy.IsChecked==true) {
                for (int cik = 0; cik < sliNevekszama.Value; cik++)
                {
                    int kivalasztottCsI = vel.Next(csaladiNevek.Count);
                    int kivalasztottUI = vel.Next(utoNevek.Count);

                    //int kivalasztottCsI = vel.Next(lbCsaladnevek.Items.Count);
                    //int kivalasztottUI = vel.Next(lbUtonevek.Items.Count);
                    lbKesznevek.Items.Add($"{csaladiNevek[kivalasztottCsI]} {utoNevek[kivalasztottUI]}");

                    csaladiNevek.RemoveAt(kivalasztottCsI);
                    utoNevek.RemoveAt(kivalasztottUI);
                    //csaladiNevek.Remove(lbCsaladnevek.SelectedItem.ToString());
                    //utoNevek.Remove(lbUtonevek.SelectedItem.ToString());
                }
            }
            else if (rbKetto.IsChecked==true)
            {
                for (int cik = 0; cik < sliNevekszama.Value; cik++)
                {
                    int kivalasztottCsI = vel.Next(csaladiNevek.Count);
                    int kivalasztottUI = vel.Next(utoNevek.Count);
                    int kivalasztottUI2 = vel.Next(utoNevek.Count - kivalasztottUI);

                    //int kivalasztottCsI = vel.Next(lbCsaladnevek.Items.Count);
                    //int kivalasztottUI = vel.Next(lbUtonevek.Items.Count);
                    //int kivalasztottUI2 = vel.Next(lbUtonevek.Items.Count - kivalasztottUI);

                    lbKesznevek.Items.Add($"{csaladiNevek[kivalasztottCsI]} {utoNevek[kivalasztottUI]} {utoNevek[kivalasztottUI2]}");

                    csaladiNevek.RemoveAt(kivalasztottCsI);
                    utoNevek.RemoveAt(kivalasztottUI);
                    utoNevek.RemoveAt(kivalasztottUI2);
                }
            }
        }

        private void btnTorol_Click(object sender, RoutedEventArgs e)
        {
            lbRendez.Visibility = Visibility.Hidden;
            foreach (var item in lbKesznevek.Items)
            {
                var felvagottItem=item.ToString().Split(' ');
                csaladiNevek.Add(felvagottItem[0]);
                utoNevek.Add(felvagottItem[1]);
                try
                {
                    utoNevek.Add(felvagottItem[2]);
                }
                catch 
                { 
                
                }
            }
            lbKesznevek.Items.Clear();
            

        }

        private void btnRendez_Click(object sender, RoutedEventArgs e)
        {
            if (lbKesznevek.Items.Count!=0)
            {
                lbRendez.Visibility = Visibility.Visible;
                lbKesznevek.Items.SortDescriptions.Add(new SortDescription("", ListSortDirection.Ascending));
            }
            
        }

        private void btnMent_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.DefaultExt = "txt";
            sfd.Filter = "Szöveges fájl (*.txt)|*.txt|CSV fájl (*.csv)|*.csv|Összes fájl (*.*)|*.*";
            sfd.Title = "Adja meg a névsor nevét!";
            if (sfd.ShowDialog() == true) {
                using (var sw = new StreamWriter(sfd.FileName, false))
                    foreach (var item in lbKesznevek.Items)
                    {
                        sw.Write(item.ToString()+"\n");
                    }
            }
        }

        private void lbKesznevek_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            var felvagottItem = lbKesznevek.SelectedItem.ToString().Split(' ');
            csaladiNevek.Add(felvagottItem[0]);
            utoNevek.Add(felvagottItem[1]);
            try
            {
                utoNevek.Add(felvagottItem[2]);
            }
            catch
            {

            }
            lbKesznevek.Items.Remove(lbKesznevek.SelectedItem);
        }
    }
}
