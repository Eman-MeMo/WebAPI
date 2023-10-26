using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Day3_Consumer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            FillEmployeesToGridView();
            FillDepartmentsToDDL();
            FillEmployeesToDDL();   //DDL --> Drop down list


        }
        private void FillEmployeesToDDL()
        {
            HttpClient client = new HttpClient();
            var result = client.GetAsync("http://localhost:9999/api/Employee").Result;  //return http response massage

            if (result.IsSuccessStatusCode)  //all 2xx family
            {
                var emps = result.Content.ReadAsAsync<List<Emp>>().Result;
                comboBox1.DataSource = emps;
                comboBox1.DisplayMember = "name";
                comboBox1.ValueMember = "id";

            }
            else
            {
                MessageBox.Show(result.StatusCode.ToString());
            }
        }

        private void FillEmployeesToGridView()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:9999");
            var result = client.GetAsync("api/Employee").Result;  //return http response massage
            var result2 = client.GetAsync("api/Department").Result;
            List<Dep> depts = result2.Content.ReadAsAsync<List<Dep>>().Result;
            if (result.IsSuccessStatusCode)  //all 2xx family
            {
                var emps = result.Content.ReadAsAsync<List<Emp>>().Result
                   .Select(emp => new
                   {
                       Id = emp.id,
                       Name = emp.name,
                       Age = emp.age,
                       Salary = emp.salary,
                       DepartmentName = depts.FirstOrDefault(d => d.id == emp.DepartmentId)?.name ?? "Unknown"
                   }).ToList();

                dataGridView1.DataSource = emps;


            }
            else
            {
                MessageBox.Show(result.StatusCode.ToString());
            }
        }

        private void FillDepartmentsToDDL()
        {
            HttpClient client = new HttpClient();
            var result = client.GetAsync("http://localhost:9999/api/Department").Result;

            if (result.IsSuccessStatusCode)
            {
                var depts = result.Content.ReadAsAsync<List<Dep>>().Result;
                comboBox3.DataSource = depts;
                comboBox3.DisplayMember = "name";
                comboBox3.ValueMember = "id";
            }
            else
            {
                MessageBox.Show(result.StatusCode.ToString());
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (comboBox3.SelectedItem != null)
            {
                int selectedDepartmentId = (int)comboBox3.SelectedValue;

                Emp obj = new Emp()
                {
                    id = int.Parse(textBox1.Text),
                    name = textBox2.Text,
                    age = int.Parse(textBox3.Text),
                    salary = decimal.Parse(textBox4.Text),
                    DepartmentId = selectedDepartmentId,
                };

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("http://localhost:9999");
                var result = httpClient.PostAsJsonAsync("api/Employee", obj).Result;

                if (result.IsSuccessStatusCode)
                {
                    FillEmployeesToGridView();
                    FillEmployeesToDDL();
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    comboBox3.Text = "CS";
                }
                else
                {
                    MessageBox.Show("Failed to add employee. Status Code: " + result.StatusCode.ToString());
                }
            }
            else
            {
                MessageBox.Show("Please select a department.");
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (comboBox3.SelectedItem != null)
            {
                int selectedDepartmentId = (int)comboBox3.SelectedValue;

                Emp obj = new Emp()
                {
                    id = int.Parse(textBox1.Text),
                    name = textBox2.Text,
                    age = int.Parse(textBox3.Text),
                    salary = decimal.Parse(textBox4.Text),
                    DepartmentId = selectedDepartmentId,
                };

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("http://localhost:9999");
                var result = httpClient.PutAsJsonAsync($"api/Employee/{textBox1.Text}", obj).Result;

                if (result.IsSuccessStatusCode)
                {
                    FillEmployeesToGridView();
                }
                else
                {
                    MessageBox.Show("Failed to update employee. Status Code: " + result.StatusCode.ToString());
                }
            }
            else
            {
                MessageBox.Show("Please select a department.");
            }
        }

        private void show_Click_1(object sender, EventArgs e)
        {
            int id = (int)comboBox1.SelectedValue;
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:9999");

            var result = httpClient.GetAsync("api/Employee").Result;
            List<Emp> emps = result.Content.ReadAsAsync<List<Emp>>().Result;
            Emp emp = emps.Find(e => e.id == id);

            var result2 = httpClient.GetAsync("api/Department").Result;
            List<Dep> depts = result2.Content.ReadAsAsync<List<Dep>>().Result;
            Dep dep = depts.Find(d => d.id == emp.DepartmentId);
            
            if (emp != null)
            {
                textBox1.Text = emp.id.ToString();
                textBox2.Text = emp.name;
                textBox3.Text = emp.age.ToString();
                textBox4.Text = emp.salary.ToString();
                if (dep != null)
                {
                    comboBox3.Text = dep.name;
                }
                else
                {
                    
                    comboBox3.Text = "CS";
                }
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {

            int id = int.Parse(textBox1.Text);
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:9999");
            var result = httpClient.DeleteAsync($"api/Employee/{id}").Result;

                if (result.IsSuccessStatusCode)
                {
                    FillEmployeesToGridView();
                    FillEmployeesToDDL();
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox3.Text = "";
                        textBox4.Text = "";
                        comboBox3.Text = "CS";
            }
                else
                {
                    MessageBox.Show("Failed to delete employee. Status Code: " + result.StatusCode.ToString());
                }
         
        }
    }
}
