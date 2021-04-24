using System;

namespace Lab4_Proxy {
  class Program {
    static void Main(string[] args) {
      SimpleGUI gui = new SimpleGUI();
      IImage image = new ImageProxy(gui);
      Console.WriteLine("Usage: [set <x> <y>] | [load]:");
      while (true) {
        string user_input = Console.ReadLine();
        if (user_input.Equals("exit"))
          return;
        if (user_input.StartsWith("set ")) {
          string[] inp = user_input.Split(" ");
          try {
            int x = int.Parse(inp[1]);
            int y = int.Parse(inp[2]);
            image.SetPos(x, y);
          }
          catch (Exception e) {
            Console.WriteLine("Invalid params");
          }
          continue;
        }

        if (user_input.Equals("load")) {
          image.DrawImage();
          continue;
        }
        Console.WriteLine("Unknown command");
      }

    }
  }

  interface IImage {
    public void DrawImage();
    public void SetPos(int x, int y);
  }

  class Image : IImage {
    public int xPos { get; protected set; } = 0;
    public int yPos { get; protected set; } = 0;
    private SimpleGUI gui;
    private string[] img;
    public Image(SimpleGUI gui) {
      this.gui = gui;
      img = new string[6] { "       ",
                            "  0 0  ",    
                            "   +   ",
                            " \\___/ ",
                            "       ",
                            "       "
                            };
    }
    public void DrawImage() {
      gui.DrawImage(xPos, yPos, img);
    }

    public void SetPos(int x, int y) {
      xPos = x;
      yPos = y;
      DrawImage();
    }
  }

  class ImageProxy : IImage {
    public int xPos { get; protected set; } = 0;
    public int yPos { get; protected set; } = 0;
    private int w = 7;
    private int h = 6;
    private SimpleGUI gui;
    private Image realImage = null;
    public ImageProxy(SimpleGUI gui) {
      this.gui = gui;
    }
    public void DrawImage() {
      if (realImage == null) {
        realImage = new Image(gui);
        realImage.SetPos(xPos, yPos);
      }
      realImage.DrawImage();
    }

    public void SetPos(int x, int y) {
      if (realImage != null) {
        realImage.SetPos(x, y);
      }
      else {
        xPos = x;
        yPos = y;
        gui.DrawFrame(xPos-1, yPos-1, w+2, h+2);
      }
    }
  }

  class SimpleGUI {
    private int x0 = 1;
    private int y0 = 3;
    private int maxH = 15;
    private int maxW = 100;
    private int cmdY;

    public SimpleGUI() {
      cmdY = y0 + maxH + 2;
      Console.WriteLine("Simple GUI:");
      Console.WriteLine("========================================================================");
      y0 = Console.CursorTop + 1;
      Console.SetCursorPosition(0, cmdY);
      Console.WriteLine("========================================================================");

    }

    public void Clear() {
      int old_cx = Console.CursorLeft;
      int old_cy = Console.CursorTop;

      Console.SetCursorPosition(0, y0-1);
      for (int y = y0 - 1; y < y0 + maxH + 2; y++) {
        Console.WriteLine(new string(' ', maxW));
      }

      Console.SetCursorPosition(old_cx, old_cy);

    }

    public void DrawFrame(int x, int y, int w, int h) {
      int old_cx = Console.CursorLeft;
      int old_cy = Console.CursorTop;
      Clear();
      y = y > maxH ? maxH : y;
      x = x > maxW ? maxW : x;
      h = y+h > maxH ? maxH-y : h;
      w = x+w > maxW ? maxW-x : w;
      int currY = y0 + y;
      int currX = x0 + x;
      Console.SetCursorPosition(currX, currY++);
      Console.Write("+" + new string('-', w - 2) + "+");
      for (int i = 0; i < h - 2; i++) {
        Console.SetCursorPosition(currX, currY++);
        Console.Write("|" + new string(' ', w - 2) + "|");
      }
      Console.SetCursorPosition(currX, currY++);
      Console.Write("+" + new string('-', w - 2) + "+");

      Console.SetCursorPosition(old_cx, old_cy);
    }
    public void DrawImage(int x, int y, string[] image) {
      int old_cx = Console.CursorLeft;
      int old_cy = Console.CursorTop;
      y = y > maxH ? maxH : y;
      x = x > maxW ? maxW : x;
      int h = y + image.GetLength(0) > maxH ? maxH - y : image.GetLength(0);
      int w = x + image[0].Length > maxW ? maxW - x : image[0].Length;
      int currY = y0 + y;
      int currX = x0 + x;
      Clear();
      DrawFrame(x - 1, y - 1, w + 2, h + 2);
      Console.SetCursorPosition(currX, currY++);
      foreach (var l in image) {
        Console.Write(l.Substring(0,w));
        if (currY > y0 + maxH)
          break;
        Console.SetCursorPosition(currX, currY++);
      }
      Console.SetCursorPosition(old_cx, old_cy);
    }
  }
}
