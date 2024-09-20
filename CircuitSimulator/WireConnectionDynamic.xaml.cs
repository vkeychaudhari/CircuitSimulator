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
using System.Windows.Shapes;

namespace CircuitSimulator
{
    /// <summary>
    /// Interaction logic for WireConnectionDynamic.xaml
    /// </summary>
    public partial class WireConnectionDynamic : Window
    {
        private Point positiveStartPoint;  // Start point for the positive wire
        private Point negativeStartPoint;  // Start point for the negative wire
        private bool positiveWireConnected = false;
        private bool negativeWireConnected = false;
        private bool positiveWireDragging = false;
        private bool negativeWireDragging = false;

        private Battery battery;
        private LightBulb lightBulb;
        private Wire positiveWire;
        private Wire negativeWire;
        private Circuit circuit;

        private bool isSwitchOn = false;

        public WireConnectionDynamic()
        {
            InitializeComponent();

            InitializeCircuit();

            // Set start points for the positive and negative terminals
            positiveStartPoint = new Point(Canvas.GetLeft(BatteryShape) + BatteryShape.Width, Canvas.GetTop(BatteryShape) + 20);  // + terminal
            negativeStartPoint = new Point(Canvas.GetLeft(BatteryShape) + BatteryShape.Width, Canvas.GetTop(BatteryShape) + 80); // - terminal

            // Event handling for dragging the wires
            CircuitCanvas.MouseLeftButtonDown += OnWireStart;
            CircuitCanvas.MouseMove += OnWireMove;
            CircuitCanvas.MouseLeftButtonUp += OnWireEnd;
        }

        private void SwitchButton_Checked(object sender, RoutedEventArgs e)
        {
            isSwitchOn = true;
            SwitchButton.Content = "Switch On";
            UpdateCircuitState();
        }

        private void SwitchButton_Unchecked(object sender, RoutedEventArgs e)
        {
            isSwitchOn = false;
            SwitchButton.Content = "Switch Off";
            UpdateCircuitState();
        }


        private void InitializeCircuit()
        {
            // Create the battery with voltage
             battery = new Battery(9); // 9V Battery

            // Create the lightbulb
             lightBulb = new LightBulb(100); // Resistance of 100 ohms

            // Create the wires and connect to the battery's OutputNode and GroundNode
             positiveWire = new Wire(battery.OutputNode, lightBulb.InputNode);
             negativeWire = new Wire(battery.GroundNode, lightBulb.InputNode);

            // Initialize the circuit
             circuit = new Circuit(battery, lightBulb, positiveWire, negativeWire);
        }

        // Start dragging the wires
        private void OnWireStart(object sender, MouseButtonEventArgs e)
        {
            Point mousePos = e.GetPosition(CircuitCanvas);
            if (IsMouseOverPositiveTerminal(mousePos))
            {
                positiveWireDragging = true;
            }
            else if (IsMouseOverNegativeTerminal(mousePos))
            {
                negativeWireDragging = true;
            }
        }

        // Move the wires dynamically
        private void OnWireMove(object sender, MouseEventArgs e)
        {
            if (positiveWireDragging)
            {
                Point mousePos = e.GetPosition(CircuitCanvas);
                DrawWire(PositiveWirePath, positiveStartPoint, mousePos);

                // Check if the wire is over the lightbulb
                positiveWireConnected = IsMouseOverLightBulb(mousePos);
            }
            else if (negativeWireDragging)
            {
                Point mousePos = e.GetPosition(CircuitCanvas);
                DrawWire(NegativeWirePath, negativeStartPoint, mousePos);

                // Check if the wire is over the lightbulb
                negativeWireConnected = IsMouseOverLightBulb(mousePos);
            }

            UpdateCircuitState();
        }

        private void DrawWire(Path wirePath, Point startPoint, Point endPoint)
        {
            PathGeometry geometry = new PathGeometry();
            PathFigure figure = new PathFigure
            {
                StartPoint = startPoint
            };

            // Calculate control points for a curve
            Point controlPoint1 = new Point((startPoint.X + endPoint.X) / 2, startPoint.Y);
            Point controlPoint2 = new Point((startPoint.X + endPoint.X) / 2, endPoint.Y);

            // Create a BezierSegment with control points
            BezierSegment bezierSegment = new BezierSegment(
                controlPoint1,
                controlPoint2,
                endPoint,
                true);

            figure.Segments.Add(bezierSegment);
            geometry.Figures.Add(figure);

            wirePath.Data = geometry;
        }

        // End dragging the wire
        private void OnWireEnd(object sender, MouseButtonEventArgs e)
        {
            positiveWireDragging = false;
            negativeWireDragging = false;

            UpdateCircuitState();
        }

        // Reset the circuit
        private void OnResetCircuit(object sender, RoutedEventArgs e)
        {
            positiveWireConnected = false;
            negativeWireConnected = false;

            // Clear the wire paths
            PositiveWirePath.Data = null;
            NegativeWirePath.Data = null;

            // Turn off the lightbulb
            LightBulbShape.Fill = new SolidColorBrush(Colors.Gray);
        }

        // Check if the mouse is over the positive terminal
        private bool IsMouseOverPositiveTerminal(Point position)
        {
            return position.X >= Canvas.GetLeft(BatteryShape) + BatteryShape.Width &&
                   position.Y >= Canvas.GetTop(BatteryShape) &&
                   position.Y <= Canvas.GetTop(BatteryShape) + 50;
        }

        // Check if the mouse is over the negative terminal
        private bool IsMouseOverNegativeTerminal(Point position)
        {
            return position.X >= Canvas.GetLeft(BatteryShape) + BatteryShape.Width &&
                   position.Y >= Canvas.GetTop(BatteryShape) + 50 &&
                   position.Y <= Canvas.GetTop(BatteryShape) + BatteryShape.Height;
        }

        // Check if the mouse is over the lightbulb
        private bool IsMouseOverLightBulb(Point position)
        {
            // Check if the mouse position (wire end) is close to the lightbulb's input node
            Rect lightBulbBounds = new Rect(Canvas.GetLeft(LightBulbShape), Canvas.GetTop(LightBulbShape), LightBulbShape.Width, LightBulbShape.Height);
            return lightBulbBounds.Contains(position);
        }
        // Update the circuit state
        private void UpdateCircuitState()
        {
            // Check if both wires are connected and the switch is on
            if (positiveWireConnected && negativeWireConnected && isSwitchOn)
            {
                // Turn on the light bulb
                lightBulb.IsOn = true;
                LightBulbShape.Fill = Brushes.Yellow; // Assuming LightBulbVisual is a visual representation of the lightbulb
            }
            else
            {
                // Turn off the light bulb
                lightBulb.IsOn = false;
                LightBulbShape.Fill = Brushes.Gray;
            }
        }
    }
}
