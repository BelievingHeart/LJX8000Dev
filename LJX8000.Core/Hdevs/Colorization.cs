//
// File generated by HDevelop for HALCON/.NET (C#) Version 18.11.0.0
// Non-ASCII strings in this file are encoded in local-8-bit encoding (cp936).
// 
// Please note that non-ASCII characters in string constants are exported
// as octal codes in order to guarantee that the strings are correctly
// created on all systems, independent on any compiler settings.
// 
// Source files with different encoding should not be mixed in one project.
//
//  This file is intended to be used with the HDevelopTemplate or
//  HDevelopTemplateWPF projects located under %HALCONEXAMPLES%\c#

using System;
using HalconDotNet;

public partial class HDevelopExport
{
  public HTuple hv_ExpDefaultWinHandle;

  // Procedures 
  // Local procedures 
  public static void ColorizeKeyenceTifImage (HObject ho_Image, out HObject ho_ImageColorized)
  {



    // Local iconic variables 

    HObject ho_Region, ho_ImageReduced, ho_ImageScaled;

    // Local control variables 

    HTuple hv_Min = new HTuple(), hv_Max = new HTuple();
    HTuple hv_Range = new HTuple(), hv_scale = new HTuple();
    // Initialize local and output iconic variables 
    HOperatorSet.GenEmptyObj(out ho_ImageColorized);
    HOperatorSet.GenEmptyObj(out ho_Region);
    HOperatorSet.GenEmptyObj(out ho_ImageReduced);
    HOperatorSet.GenEmptyObj(out ho_ImageScaled);
    ho_Region.Dispose();
    HOperatorSet.Threshold(ho_Image, out ho_Region, 1, 999999);
    ho_ImageReduced.Dispose();
    HOperatorSet.ReduceDomain(ho_Image, ho_Region, out ho_ImageReduced);
    hv_Min.Dispose();hv_Max.Dispose();hv_Range.Dispose();
    HOperatorSet.MinMaxGray(ho_Region, ho_ImageReduced, 0, out hv_Min, out hv_Max, 
        out hv_Range);
    hv_scale.Dispose();
    using (HDevDisposeHelper dh = new HDevDisposeHelper())
    {
    hv_scale = 65535.0/hv_Range;
    }
    using (HDevDisposeHelper dh = new HDevDisposeHelper())
    {
    ho_ImageScaled.Dispose();
    HOperatorSet.ScaleImage(ho_Image, out ho_ImageScaled, 1, -hv_Min);
    }
    ho_ImageColorized.Dispose();
    HOperatorSet.ScaleImage(ho_ImageScaled, out ho_ImageColorized, hv_scale, 0);
    ho_Region.Dispose();
    ho_ImageReduced.Dispose();
    ho_ImageScaled.Dispose();

    hv_Min.Dispose();
    hv_Max.Dispose();
    hv_Range.Dispose();
    hv_scale.Dispose();

    return;
  }

  // Main procedure 
  private void action()
  {

    // Initialize local and output iconic variables 
    //dev_set_lut ('six')
    //read_image (Image, 'D:/share/3DImages/1115/Correlation/2��/2/1115-094557-1590.tif')
    //ColorizeKeyenceTifImage (Image, ImageScaled1)



  }



}

