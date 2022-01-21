using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Play375
{

    /// <summary>
    /// 随机选择行，随机取牙签,一人一次,
    /// </summary>
    public partial class Form1 : Form
    {
        private delegate void setTxtContent(string msg);
        Dictionary<int, List<int>> pokerBox;
        Random ran;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pokerBox = new Dictionary<int, List<int>>();
            ran = new Random();
        }
        /// <summary>
        /// 开始按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBegin_Click(object sender, EventArgs e)
        {
           try
            {
                txtInfo.Clear();
                pokerBoxinit();//牙签盒子里装牙签
                Getpoker();//取牙签
            }
            catch (Exception ex)
            {
                this.Invoke(new setTxtContent(ShowLog), new object[] { ex.Message });
            }
            
           
        }

        /// <summary>
        /// 初始化数据（也可以通过抽象出从接口的方式去获取数据，这样数据就可以扩展）
        /// </summary>
        /// <returns></returns>
        private void pokerBoxinit()
        {
            pokerBox.Clear();
            pokerBox.Add(1, new List<int>() { 1, 2, 3 });
            pokerBox.Add(2, new List<int>() { 1, 2, 3, 4, 5 });
            pokerBox.Add(3, new List<int>() { 1, 2, 3, 4, 5, 6, 7 });
        }

        /// <summary>
        /// 在有牙签盒子里一人一次拿牙签
        /// </summary>
        private void Getpoker()
        {
            int userSelectedRow ;
            bool userOnedo = true;
            while (pokerCount() > 0)
            {
                userSelectedRow = ran.Next(1, pokerBox.Count + 1);
                if (userOnedo)
                {
                    if (pokerBox[userSelectedRow].Count > 0)
                    {
                        userOneTake(userSelectedRow);
                        userOnedo =false;
                    }
                }
                else
                {
                    if (pokerBox[userSelectedRow].Count > 0)
                    {
                        userTwoTake(userSelectedRow);
                        userOnedo = true;
                    }
                }

            }
        }
        /// <summary>
        /// 用户一拿牙签
        /// </summary>
        /// <param name="userOneSelectedRow"></param>
        /// <returns></returns>
        private void userOneTake(int userOneSelectedRow)
        {
            GetpokerCaculate(userOneSelectedRow, Operation.userOne);
        }
        /// <summary>
        /// 用户二拿牙签
        /// </summary>
        /// <param name="userOneSelectedRow"></param>
        /// <returns></returns>
        private void userTwoTake(int userTwoSelectedRow)
        {
            GetpokerCaculate(userTwoSelectedRow, Operation.userTwo);
        }

        /// <summary>
        /// 取总牙签数量
        /// </summary>
        /// <returns>总牙签数量</returns>
        private int pokerCount()
        {
            int sum = 0;
            for (int index = 1; index <= pokerBox.Count; index++)
            {
                sum = sum + pokerBox[index].Count;
            }
            return sum;
        }

        /// <summary>
        /// 计算牙签
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="operation"></param>
        private void GetpokerCaculate(int rowIndex, Operation operation)
        {
            string msg = string.Empty;
            List<int> lstSelectedRowData = pokerBox[rowIndex];
            int selectCount = ran.Next(1, lstSelectedRowData.Count + 1);
            if (selectCount == pokerCount() && selectCount != 1) //使得对手取最后一根
                selectCount--;
            pokerBox[rowIndex].RemoveRange(0, selectCount);
            if (pokerBox[rowIndex].Count == 0)
            {
                msg = string.Format("User {0} get others {1} at row {2}", Enum.GetName(typeof(Operation), operation), selectCount, rowIndex);
                this.Invoke(new setTxtContent(ShowLog), new object[] { msg });
                if (pokerCount() == 0)//最后一根取完列表中的牙签判定为输
                {
                    string content = string.Empty;
                    if (operation == Operation.userOne)
                    {
                        content = "userOne lose";
                    }
                    else
                    {
                        content = "userTwo lose";
                    }
                    this.Invoke(new setTxtContent(ShowLog), new object[] { content });
                }
            }
            else
            {
                msg = string.Format("User {0} get {1} at row {2}", Enum.GetName(typeof(Operation), operation), selectCount, rowIndex);
                this.Invoke(new setTxtContent(ShowLog), new object[] { msg });
            }
        }

        /// <summary>
        /// 界面显示结果
        /// </summary>
        /// <param name="msg"></param>
        private void ShowLog(string msg)
        {
            if (!string.IsNullOrEmpty(txtInfo.Text))
            {
                txtInfo.AppendText("\r\n");
            }
            txtInfo.AppendText(msg);
        }


    }
    /// <summary>
    /// 操作人枚举
    /// </summary>
    public enum Operation
    {
        userOne,
        userTwo
    }
}
