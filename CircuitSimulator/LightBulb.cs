using System;
using System.Xml.Linq;

namespace CircuitSimulator
{
    public class Resistor
    {
        public double Resistance { get; set; }
        public Node OutputNode { get; set; }
        public Node InputNode { get; set; }

        public Resistor(double resistance)
        {
            Resistance = resistance;
            OutputNode = new Node();
            InputNode = new Node();
        }

        public double CalculateVoltageDrop(double current)
        {
            return current * Resistance;
        }
    }



    public class Battery
    {
        public Node OutputNode { get; set; }  // Positive terminal
        public Node GroundNode { get; set; }  // Negative terminal
        public double Voltage { get; set; }

        public Battery(double voltage)
        {
            Voltage = voltage;
            OutputNode = new Node();
            GroundNode = new Node();
        }
    }


    public class LightBulb
{
    public bool IsOn { get; set; }
    public double Resistance { get; set; }
    public Node InputNode { get; set; }

    public LightBulb(double resistance)
    {
        Resistance = resistance;
        IsOn = false;
    }
}

    public class Node
    {
        public bool IsConnected { get; set; }

        public Node()
        {
            IsConnected = false;
        }
    }

    public class Wire
    {
        public bool IsConnected { get; set; }
        public Node StartNode { get; set; }
        public Node EndNode { get; set; }

        public Wire(Node start, Node end)
        {
            StartNode = start;
            EndNode = end;
            IsConnected = false;
        }

        // Logic for connecting wire
        public void Connect()
        {
            IsConnected = true;
        }

        public void Disconnect()
        {
            IsConnected = false;
        }
    }
    public class Circuit
    {
        public Battery Battery { get; set; }
        public LightBulb LightBulb { get; set; }
        public Wire PositiveWire { get; set; }
        public Wire NegativeWire { get; set; }

        // Constructor that takes 4 arguments
        public Circuit(Battery battery, LightBulb lightBulb, Wire positiveWire, Wire negativeWire)
        {
            Battery = battery;
            LightBulb = lightBulb;
            PositiveWire = positiveWire;
            NegativeWire = negativeWire;
        }

        // Method to calculate the current and check if the circuit is complete
        public void CalculateCurrent()
        {
            // Check if both wires are connected properly
            if (PositiveWire.IsConnected && NegativeWire.IsConnected)
            {
                // Logic to calculate the current in the circuit and update the lightbulb state
                LightBulb.IsOn = true;
            }
            else
            {
                LightBulb.IsOn = false;
            }
        }
    }




}