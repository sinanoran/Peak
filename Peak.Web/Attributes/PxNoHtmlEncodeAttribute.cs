﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Web.Attributes {
  [AttributeUsage(AttributeTargets.Property)]
  public class PxNoHtmlEncodeAttribute : Attribute {
  }
}
