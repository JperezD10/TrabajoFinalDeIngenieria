﻿using SEGURIDAD;
using System;

namespace Artify
{
    public partial class Mantenimiento : BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SecurityManager.CheckAccess(this);
            RegisterLocalizablesById(this, "maint");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}