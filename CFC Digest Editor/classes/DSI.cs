using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CFC_Digest_Editor.classes
{
    public class DSI
    {
        const int FolderSize = 0x40000;
        const int HeaderSize = 0x40;

        public static void ExtractAndMerge(string inputPath, string outputVideoPath, string outputAudioPath)
        {
             var stream = new FileStream(inputPath, FileMode.Open, FileAccess.Read);
             var videoOutput = new FileStream(outputVideoPath, FileMode.Create, FileAccess.Write);
             var audioOutput = new FileStream(outputAudioPath, FileMode.Create, FileAccess.Write);

            int folderIndex = 0;

            while (stream.Position + FolderSize <= stream.Length)
            {
                byte[] folderData = new byte[FolderSize];
                stream.Read(folderData, 0, FolderSize);

                 var ms = new MemoryStream(folderData);
                 var br = new BinaryReader(ms);

                int streamCount = br.ReadInt32();
                int m2vStartOffset = br.ReadInt32();
                int block1ID = (int)folderData.ReadUInt(0x08,16); // Desconhecido
                br.ReadInt32();
                int m2vSize = br.ReadInt32();
                int vagStartOffset = br.ReadInt32();
                int block2ID = (int)folderData.ReadUInt(0x14, 16); // Desconhecido
                br.ReadInt32();
                int vagSize = br.ReadInt32();

                ms.Position = m2vStartOffset;
                byte[] m2vData = br.ReadBytes(m2vSize);

                ms.Position = vagStartOffset;
                byte[] vagData = br.ReadBytes(vagSize);

                if (block1ID != 0xC000)
                {
                    videoOutput.Write(vagData, 0, vagData.Length);
                    audioOutput.Write(m2vData, 0, m2vData.Length);
                }
                else
                {
                    videoOutput.Write(m2vData, 0, m2vData.Length);
                    audioOutput.Write(vagData, 0, vagData.Length);
                }

                folderIndex++;
            }
            videoOutput.Close();
            audioOutput.Close();
            MessageBox.Show("Extracted sucessfully!", "Action", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //if (MessageBox.Show("Want to convert vag to wav?", "Question",
            //   MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //    Convert(outputAudioPath, Path.ChangeExtension(outputAudioPath, ".wav"));

        }
        public static void BuildDSIFromStreams(string m2vPath, string vagPath, string outputDsiPath)
        {
            const int FolderSize = 0x40000;
            const int HeaderSize = 0x40;
            const int MaxAudioSize = 0xB000;
            const int MinGapBetweenStreams = 0x1000;

            FileStream video = new FileStream(m2vPath, FileMode.Open, FileAccess.Read);
            FileStream audio = new FileStream(vagPath, FileMode.Open, FileAccess.Read);
            FileStream output = new FileStream(outputDsiPath, FileMode.Create, FileAccess.Write);

            try
            {
                while (video.Position < video.Length || audio.Position < audio.Length)
                {
                    int maxAvailableForVideo = FolderSize - HeaderSize - MaxAudioSize - MinGapBetweenStreams;
                    byte[] m2vData = ReadBlock(video, maxAvailableForVideo);
                    byte[] vagData = ReadBlock(audio, MaxAudioSize);

                    int m2vSize = m2vData.Length;
                    int vagSize = vagData.Length;

                    int m2vOffset = HeaderSize;
                    int vagOffset = m2vOffset + m2vSize + MinGapBetweenStreams;

                    byte[] folder = new byte[FolderSize];
                    MemoryStream ms = new MemoryStream(folder, 0, FolderSize, true, true);
                    BinaryWriter bw = new BinaryWriter(ms);

                    // Header atualizado
                    bw.Write(2);                       // StreamCount
                    bw.Write(m2vOffset);               // Offset do bloco de vídeo
                    bw.Write((ushort)0xC000);          // ID do bloco de vídeo
                    bw.Write((ushort)0xFFFF);          // Reservado
                    bw.Write(m2vSize);                 // Tamanho do vídeo
                    bw.Write(vagOffset);               // Offset do bloco de áudio
                    bw.Write((ushort)0xE000);          // ID do bloco de áudio
                    bw.Write((ushort)0xFFFF);          // Constante
                    bw.Write(vagSize);                 // Tamanho do áudio
                    bw.Write(0);                       // Reservado

                    // Preencher até 0x40
                    while (ms.Position < HeaderSize)
                        ms.WriteByte(0);

                    // Dados
                    ms.Position = m2vOffset;
                    ms.Write(m2vData, 0, m2vSize);

                    ms.Position = vagOffset;
                    ms.Write(vagData, 0, vagSize);

                    output.Write(folder, 0, FolderSize);

                    bw.Dispose();
                    ms.Dispose();
                }
            }
            finally
            {
                video.Dispose();
                audio.Dispose();
                output.Dispose();
            }

            MessageBox.Show("DSI rebuild successful!", "Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
           
        }

        private static byte[] ReadBlock(FileStream fs, int maxSize)
        {
            if (fs.Position >= fs.Length)
                return Array.Empty<byte>();

            int remaining = (int)Math.Min(maxSize, fs.Length - fs.Position);
            byte[] buffer = new byte[remaining];
            fs.Read(buffer, 0, remaining);
            return buffer;
        }

        public static void Convert(string inputPath, string outputPath)
        {
            byte[] adpcm = File.ReadAllBytes(inputPath);
            byte[] pcm = ADPCM.ToPCMStereo(adpcm, adpcm.Length, 16);

            File.WriteAllBytes(outputPath,SaveRiff(pcm, 2, 48000));
            MessageBox.Show("Converted sucessfully!!", "Action");
        }

        public static byte[] SaveRiff(byte[] pcm, short channels, int samplerate)
        {
            byte[] data = new byte[pcm.Length + 44];
            BinaryWriter writer = new BinaryWriter(new MemoryStream(data));
            writer.Write("RIFF".ToCharArray());
            writer.Write(36 + pcm.Length);
            writer.Write("WAVE".ToCharArray());
            writer.Write("fmt ".ToCharArray());
            writer.Write(16);
            writer.Write((ushort)1);
            writer.Write(channels);
            writer.Write(samplerate);
            writer.Write(samplerate * channels * 2);
            writer.Write((short)(channels * 2));
            writer.Write((ushort)16);
            writer.Write("data".ToCharArray());
            writer.Write(pcm.Length);
            writer.Write(pcm);
            writer.Close();
            return data;
        }
    

    }
}
