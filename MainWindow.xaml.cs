using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Drawing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //Establishes initial values for pen, which are set on initialize.
        private readonly DrawingAttributes PenValues = new DrawingAttributes()
        {
            Color = Colors.Black,
            Height = 20,
            Width = 20
        };

        //Initialize, as well as define default settings to reflect what is above.
        public MainWindow()
        {
            InitializeComponent();
            drawRegion.DefaultDrawingAttributes = PenValues;
        }


        //Sets brush mode based on user input
        private void brush_Choice(object sender, EventArgs e)
        {
            System.Windows.Controls.Image chosenButton = new Image();

            chosenButton = (System.Windows.Controls.Image)sender;

            //Creates decisions based on the selected image button. Impacts brush behavior
            if (chosenButton.Name == "paintBrush" )
            {
                enumChoice(mouseBehavior.Brush);
            }
            else if (chosenButton.Name == "eraser")
            {
                enumChoice(mouseBehavior.Erase);
            }
        }

        //Changes brush color to be the same as the given value reflected by color blocks in makeshift color palette.
        private void color_Choice(object sender, EventArgs e)
        {
            System.Windows.Controls.Image colorPicker = new Image();

            colorPicker = (System.Windows.Controls.Image)sender;

            if (colorPicker.Name == "redBrush")
            {
                PenValues.Color = Colors.Red;
            }
            else if (colorPicker.Name == "orangeBrush")
            {
                PenValues.Color = Colors.Orange;
            }
            else if (colorPicker.Name == "yellowBrush")
            {
                PenValues.Color = Colors.Yellow;
            }
            else if (colorPicker.Name == "greenBrush")
            {
                PenValues.Color = Colors.Green;
            }
            else if (colorPicker.Name == "blueBrush")
            {
                PenValues.Color = Colors.Blue;
            }
            else if (colorPicker.Name == "purpleBrush")
            {
                PenValues.Color = Colors.Purple;
            }
            else if (colorPicker.Name == "pinkBrush")
            {
                PenValues.Color = Colors.Pink;
            }
            else if (colorPicker.Name == "blackBrush")
            {
                PenValues.Color = Colors.Black;
            }
            else if (colorPicker.Name == "whiteBrush")
            {
                PenValues.Color = Colors.White;
            }
            else if (colorPicker.Name == "grayBrush")
            {
                PenValues.Color = Colors.Gray;
            }
        }

        //Brush Size alteration. Effects width and height equally
        private void brushSize_Select(object sender, EventArgs e)
        {

            if (IsLoaded)
                PenValues.Width = brushSize.Value;
                PenValues.Height = brushSize.Value;
                //Resizes eraser. Works, but requires changing brush mode due to only updating on editing mode change.
                drawRegion.EraserShape = new EllipseStylusShape(brushSize.Value,brushSize.Value);
        }

        //Sets the mode behaviors. Done through a switch and break method I found example of.
        private void enumChoice(mouseBehavior mode)
        {
            switch (mode)
            {
                case mouseBehavior.Brush:
                    drawRegion.EditingMode = InkCanvasEditingMode.Ink;
                    drawRegion.DefaultDrawingAttributes = PenValues;
                    break;

                //Eraser behavior is mostly consistent regardless of size. It will always delete entire nearby area, not chunks.
                //This is because ink canvas is a vector program and so can't break things into pixels.
                case mouseBehavior.Erase:
                    drawRegion.EditingMode = InkCanvasEditingMode.EraseByPoint;
                    break;
            }
        }

        //Creates a list of options to pick from as a brush behavioral setting. Might add a select feature just to give it more to work with.
        //Originally planned to have a fill and several shape choices, but could not find a method that works with ink canvas.
        public enum mouseBehavior
        {
            Brush, Erase
        }

        //Give user the option to save their creation or note and or load it or something else.
        private void buttonPress(object sender, EventArgs e)
        {
            System.Windows.Controls.Button selectSaveLoad = new Button();

            selectSaveLoad = (System.Windows.Controls.Button)sender;

            if (selectSaveLoad.Name == "save")
            {
               
                //string subpath = Directory.GetCurrentDirectory();

                //Initiates file saving, limiting to set file type options
                SaveFileDialog saveCreation = new SaveFileDialog();
                saveCreation.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|Png File|*.png";
                saveCreation.Title = "Save an Image File";
                saveCreation.ShowDialog();

                //Checks if a name was given
                if (saveCreation.FileName == "") return;
                //subpath = saveCreation.FileName.Substring(0, saveCreation.FileName.Length - saveCreation.SafeFileName.Length);

                //Saves what is in the canvas
                FileStream file = File.Open(saveCreation.FileName, FileMode.OpenOrCreate);
                drawRegion.Strokes.Save(file);
                file.Close();

            }
            else if (selectSaveLoad.Name == "load")
            {

                //Open dialog box for searching for a proper file type
                OpenFileDialog openCreation = new OpenFileDialog();
                openCreation.Title = "Load Image File";
                openCreation.DefaultExt = "jpg";
                //Note: Ink Canvas works with vector drawing. None of what it makes works with bitmaps or pixels.
                //As such the file types below are not really what it will become, and only this program can open it.
                openCreation.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|Png File|*.png";
                openCreation.ShowDialog();

                //checks for name
                if (openCreation.FileName == "") return;

                //Establishes and opens file, converting and placing it in canvas
                FileStream file = File.Open(openCreation.FileName, FileMode.Open);
                StrokeCollection strokeCol = new StrokeCollection(file);
                drawRegion.Strokes = strokeCol;
                file.Close();
            }
        }

    }
}
