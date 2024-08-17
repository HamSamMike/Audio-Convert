using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;

namespace Convert
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {//选择输入文件夹
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = dialog.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {//选择输出文件夹
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = dialog.SelectedPath;
            }
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private async void button3_Click(object sender, EventArgs e)
        {//开始转换
            string inputFolder = textBox1.Text;
            string outputFolder = textBox2.Text;

            if (string.IsNullOrEmpty(inputFolder) || string.IsNullOrEmpty(outputFolder))
            {//检查文件夹是否为空
                MessageBox.Show("请先选择输入和输出文件夹！");
                return;
            }

            await Task.Run(() => ConvertMp3ToWav(inputFolder, outputFolder));
            MessageBox.Show("转换完成！");
        }

        private void ConvertMp3ToWav(string sourceDirectory, string destinationDirectory)
        {//转换函数
            if (!Directory.Exists(destinationDirectory))
            {//检查输出目录是否存在
                Directory.CreateDirectory(destinationDirectory);//如果不存在，则创建这个目录
            }

            // 获取MP3文件总数
            string[] mp3Files = Directory.GetFiles(sourceDirectory, "*.mp3");
            int totalFiles = mp3Files.Length;
 
            // 设置进度条的最大值
            Invoke(new Action(() =>
            {
                progressBar1.Minimum = 0;
                progressBar1.Maximum = totalFiles;
                progressBar1.Value = 0;
            }));

            for (int i = 0; i < totalFiles; i++)
            {//遍历原文件夹中所有mp3格式的音频文件
                string filePath = mp3Files[i];
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string destinationPath = Path.Combine(destinationDirectory, fileName + ".wav");//构造每个文件对应的目标WAV文件路径
                //下面三行代码需要NAudio包
                using (var mp3Reader = new Mp3FileReader(filePath))//读取解析MP3文件格式并解码为PCM数据流
                using (var waveStream = WaveFormatConversionStream.CreatePcmStream(mp3Reader))//将MP3数据流转换为PCM格式的WAV数据流。
                using (var waveFileWriter = new WaveFileWriter(destinationPath, waveStream.WaveFormat))
                {//将音频数据写入WAV文件中
                    waveStream.CopyTo(waveFileWriter);
                }
            

            // 更新进度
            Invoke(new Action(() =>
            {
                progressBar1.Value = i + 1;
            }));
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
