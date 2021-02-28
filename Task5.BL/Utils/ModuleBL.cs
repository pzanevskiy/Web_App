using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using Task5.DAL.UnitOfWork;
using Task5.DAL.UnitOfWork.Interfaces;

namespace Task5.BL.Util
{
    public class ModuleBL : NinjectModule
    {
        public override void Load()
        {
            Bind<IUnitOfWork>().To<EFUnitOfWork>();
        }
    }
}
