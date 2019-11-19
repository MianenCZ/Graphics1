using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mianen.Utils
{
  class MultyThreadMergeSort<T>
  {
    public const int MergeLowerLimit = 16;

    public T[] Values { get; private set; }
    public int UpperLimit { get; private set; }
    public int LowerLimit { get; private set; }
    public string SID { get; private set; }

    public IComparer Comparer { get; private set; }
    public IComparer<T> ComparerT { get; private set; }
    public MergeControler Controler { get; private set; }

    public MultyThreadMergeSort (T[] Values, IComparer Comparer, int ThreadLimit)
    {
      this.Values = Values;
      this.UpperLimit = Values.Length;
      this.LowerLimit = 0;
      this.SID = "";
      this.Comparer = Comparer;

      this.Controler = new MergeControler(ThreadLimit);
    }

    private MultyThreadMergeSort (T[] Values, IComparer Comparer, MergeControler Controler, int LowerLimit, int UpperLimit, string SID)
    {
      this.Values = Values;
      this.Comparer = Comparer;
      this.UpperLimit = UpperLimit;
      this.LowerLimit = LowerLimit;
      this.SID = SID;
      this.Controler = Controler;
    }

    public MultyThreadMergeSort (T[] Values, IComparer<T> ComparerT, int ThreadLimit)
    {
      this.Values = Values;
      this.UpperLimit = Values.Length;
      this.LowerLimit = 0;
      this.SID = "";
      this.ComparerT = ComparerT;

      this.Controler = new MergeControler(ThreadLimit);
    }

    private MultyThreadMergeSort (T[] Values, IComparer<T> ComparerT, MergeControler Controler, int LowerLimit, int UpperLimit, string SID)
    {
      this.Values = Values;
      this.ComparerT = ComparerT;
      this.UpperLimit = UpperLimit;
      this.LowerLimit = LowerLimit;
      this.SID = SID;
      this.Controler = Controler;
    }

    public void Sort ()
    {
      if (Comparer is object)
      {
        DoSort();
      }
      else if (ComparerT is object)
      {
        DoSortT();
      }
    }
    private void DoSort ()
    {
      string Lname = this.SID + "L";
      string Rname = this.SID + "R";

      if (Controler[Lname] && Controler[Rname])
      {
        int LimitCenter = (LowerLimit + UpperLimit) / 2;
        MultyThreadMergeSort<T> m1 = new MultyThreadMergeSort<T>(Values, Comparer, Controler, LowerLimit, LimitCenter, Lname);
        MultyThreadMergeSort<T> m2 = new MultyThreadMergeSort<T>(Values, Comparer, Controler, LimitCenter, UpperLimit, Rname);
        Thread t = new Thread(m1.DoSort);
        t.Start();
        m2.DoSort();
        t.Join();
        DoMerge();
      }
      else if (!(Controler[Lname] && Controler[Rname]))
      {
        this.DoQuickSort();
      }
      return;
    }
    
    private void DoQuickSort ()
    {
      T[] tmpAr = new T[UpperLimit - LowerLimit];
      Array.Copy(Values, LowerLimit, tmpAr, 0, tmpAr.Length);
      Array.Sort(tmpAr, Comparer);
      Array.Copy(tmpAr, 0, Values, LowerLimit, tmpAr.Length);
      tmpAr = null;
      return;
    }

    private void DoMerge ()
    {
      T[] tmpAr = new T[UpperLimit - LowerLimit];
      int lActual = LowerLimit;
      int center = (LowerLimit + UpperLimit) / 2;
      int pActual = center;


      for (int i = 0; i < tmpAr.Length; i++)
      {
        if (lActual == center)
        {
          //Left part is fully used
          tmpAr[i] = Values[pActual];
          pActual++;
          continue;
        }

        if (pActual == UpperLimit)
        {
          //right part is fully used
          tmpAr[i] = Values[lActual];
          lActual++;
          continue;
        }

        if (this.Comparer.Compare(Values[lActual], Values[pActual]) <= 0)
        {
          tmpAr[i] = Values[lActual];
          lActual++;
        }
        else
        {
          tmpAr[i] = Values[pActual];
          pActual++;
        }
      }
      Array.Copy(tmpAr, 0, Values, LowerLimit, tmpAr.Length);
    }

    private void DoSortT ()
    {
      string Lname = this.SID + "L";
      string Rname = this.SID + "R";

      if (Controler[Lname] && Controler[Rname])
      {
        int LimitCenter = (LowerLimit + UpperLimit) / 2;
        MultyThreadMergeSort<T> m1 = new MultyThreadMergeSort<T>(Values, ComparerT, Controler, LowerLimit, LimitCenter, Lname);
        MultyThreadMergeSort<T> m2 = new MultyThreadMergeSort<T>(Values, ComparerT, Controler, LimitCenter, UpperLimit, Rname);
        Thread t = new Thread(m1.DoSortT);
        t.Start();
        m2.DoSortT();
        t.Join();
        DoMergeT();
      }
      else if (!(Controler[Lname] && Controler[Rname]))
      {
        this.DoQuickSortT();
      }
      return;
    }

    private void DoQuickSortT ()
    {
      T[] tmpAr = new T[UpperLimit - LowerLimit];
      Array.Copy(Values, LowerLimit, tmpAr, 0, tmpAr.Length);
      Array.Sort(tmpAr, ComparerT);
      Array.Copy(tmpAr, 0, Values, LowerLimit, tmpAr.Length);
      tmpAr = null;
      return;
    }

    private void DoMergeT ()
    {
      T[] tmpAr = new T[UpperLimit - LowerLimit];
      int lActual = LowerLimit;
      int center = (LowerLimit + UpperLimit) / 2;
      int pActual = center;


      for (int i = 0; i < tmpAr.Length; i++)
      {
        if (lActual == center)
        {
          //Left part is fully used
          tmpAr[i] = Values[pActual];
          pActual++;
          continue;
        }

        if (pActual == UpperLimit)
        {
          //right part is fully used
          tmpAr[i] = Values[lActual];
          lActual++;
          continue;
        }

        if (this.ComparerT.Compare(Values[lActual], Values[pActual]) <= 0)
        {
          tmpAr[i] = Values[lActual];
          lActual++;
        }
        else
        {
          tmpAr[i] = Values[pActual];
          pActual++;
        }
      }
      Array.Copy(tmpAr, 0, Values, LowerLimit, tmpAr.Length);
    }


    internal class MergeControler
    {
      public bool this[string SID]
      {
        get
        {
          //Console.WriteLine("DataSet ask: {0}, Answer: {1}", SID, DataSet.Contains(SID));
          return DataSet.Contains(SID);
        }
      }

      private HashSet<string> DataSet;

      public MergeControler (int ThreadCount)
      {
        this.DataSet = new HashSet<string>();
        int Level = (int)Math.Ceiling(Math.Log(ThreadCount, 2));
        for (int i = 0; i < Level; i++)
        {
          Fill(i, (int)Math.Pow(2, i));
        }
        Fill(Level, ThreadCount);
      }

      public void Fill (int Level, int Limit)
      {
        int[] Limits = new int[2];
        Limits[0] = (int)Math.Pow(2, Level) / 2;
        Limits[1] = Limit - Limits[0];

#if DEBUG
        Console.WriteLine("Level: {0}", Level);
#endif
        for (int k = 0; k < 2; k++)
        {
          for (int i = 0; i < Limits[k]; i++)
          {
            string Line = "";
            for (int j = 0; j < Level; j++)
            {
              Line = ((((1 << j) & (2 * i) + k) == 0) ? "L" : "R") + Line;
            }

#if DEBUG
            Console.WriteLine(Line);
#endif
            DataSet.Add(Line);
          }
        }
      }
    }
  }
}
