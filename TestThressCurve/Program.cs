using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestThressCurve
{
    class Program
    {
         /*   
  N：已知节点数N+1   
  R：欲求插值点数R+1   
  x，y为给定函数f(x)的节点值{x(i)}   (x(i)<x(i+1))   ,以及相应的函数值{f(i)}     0<=i<=N   
  P0=f(x0)的二阶导数；Pn=f(xn)的二阶导数   
  u:存插值点{u(i)}       0<=i<=R   
  求得的结果s(ui)放入s[R+1]       0<=i<=R   
  返回0表示成功，1表示失败   
  */   
  static int   SPL(int   N,int   R,double   []x,double   []y, double P0, double Pn, double []u, double []s)   
  {   
	  double[] h = new double[N];
      double[] a = new double[N + 1];
      double[] c = new double[N];
      double[] g = new double[N + 1];
      double[] af = new double[N + 1];
      double[] ba = new double[N];
      double[] m = new double[N + 1];

      int   i,k;   
      double   p1,p2,p3,p4; 

	  /*第一步：计算方程组的系数*/   
	  for(k=0;k<N;k++)   
	    h[k]=x[k+1]-x[k];   
	  for(k=1;k<N;k++)   
	    a[k]=h[k]/(h[k]+h[k-1]);   
	  for(k=1;k<N;k++)   
	    c[k]=1-a[k];   
	  for(k=1;k<N;k++)   
	      g[k]=3*(c[k]*(y[k+1]-y[k])/h[k]+a[k]*(y[k]-y[k-1])/h[k-1]);   
	      c[0]=a[N]=1;   
	      g[0]=3*(y[1]-y[0])/h[0]-P0*h[0]/2;   
	      g[N]=3*(y[N]-y[N-1])/h[N-1]+Pn*h[N-1]/2;   
	    
	  /*第二步：用追赶法解方程组求{m(i)}     */   
	  ba[0]=c[0]/2;   
	  g[0]=g[0]/2;   
	  for(i=1;i<N;i++)         
	  {   
	      af[i]=2-a[i]*ba[i-1];   
	      g[i]=(g[i]-a[i]*g[i-1])/af[i];   
	      ba[i]=c[i]/af[i];   
	  }   
	  af[N]=2-a[N]*ba[N-1];   
	  g[N]=(g[N]-a[N]*g[N-1])/af[N];   
	    
	  m[N]=g[N];                   /*P110   公式：6.32*/   
	  for(i=N-1;i>=0;i--)   
	  m[i]=g[i]-ba[i]*m[i+1];   
	    
	  /*第三步：求值*/   
	  for(i=0;i<=R;i++)   
	  {   
	      /*判断u(i)属于哪一个子区间，即确定k   */   
	      if(u[i]<x[0]   ||   u[i]>x[N])       
	      {  
	            return   1;   
	      }   
	      k=0;   
	      while(u[i]>x[k+1])   
	        k++;   
    	    
    	    
	      p1=(h[k]+2*(u[i]-x[k]))*Math.Pow((u[i]-x[k+1]),2)*y[k]/Math.Pow(h[k],3);
          p2 = (h[k] - 2 * (u[i] - x[k + 1])) * Math.Pow((u[i] - x[k]), 2) * y[k + 1] / Math.Pow(h[k], 3);
          p3 = (u[i] - x[k]) * Math.Pow((u[i] - x[k + 1]), 2) * m[k] / Math.Pow(h[k], 2);
          p4 = (u[i] - x[k + 1]) * Math.Pow((u[i] - x[k]), 2) * m[k + 1] / Math.Pow(h[k], 2);   
	      s[i]=p1+p2+p3+p4;   
	  }   
	  
	  return   0;   
  }   

        static void Main(string[] args)
        {
            double[] x = new double[] { 0.5, 0.7, 1.9, 1.1, 1.3, 1.1, 1.9, 0.7 };
            double[] y = new double[] { 0.4794, 0.6442, 0.7833, 0.8912, 0.9636, 0.9975, 0.9917, 0.9463 };
            double[] u = new double[] { 0.6, 0.8, 1.0, 1.2, 1.4, 1.6, 1.8, 1.82, 1.83, 1.84, 1.85, 1.84, 1.8, 1.6, 1.2, 1.4 };

            double[] s = new double[u.Length];

            SPL(x.Length-1, s.Length-1, x, y, 0, 0, u, s);

            for (int i = 0; i < u.Length; i++)
                Console.Write(u[i] + "  ");

            Console.WriteLine();

            for (int i = 0; i < s.Length; i++)
                Console.Write(s[i] + " ");

            Console.Read();
        }
    }
}
