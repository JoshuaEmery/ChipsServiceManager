using CSMWebCore.Data;
using CSMWebCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public class UnitOfWork : IDisposable
    {
        private ChipsDbContext context;
        private GenericRepository<Customer> customerRepository;
        private GenericRepository<Device> deviceRepository;
        private GenericRepository<Ticket> ticketRepository;
        private GenericRepository<Log> logRepository;
        private GenericRepository<Event> eventRepository;


        public UnitOfWork(ChipsDbContext context)
        {
            this.context = context;
        }

        public GenericRepository<Customer> CustomerRepository
        {
            get
            {
                if (customerRepository == null)
                {
                    customerRepository = new GenericRepository<Customer>(context);
                }
                return customerRepository;
            }
        }

        public GenericRepository<Device> DeviceRepository
        {
            get
            {
                if (deviceRepository == null)
                {
                    deviceRepository = new GenericRepository<Device>(context);
                }
                return deviceRepository;
            }
        }

        public GenericRepository<Ticket> TicketRepository
        {
            get
            {
                if (ticketRepository == null)
                {
                    ticketRepository = new GenericRepository<Ticket>(context);
                }
                return ticketRepository;
            }
        }

        public GenericRepository<Log> LogRepository
        {
            get
            {
                if (logRepository == null)
                {
                    logRepository = new GenericRepository<Log>(context);
                }
                return logRepository;
            }
        }

        public GenericRepository<Event> EventRepository
        {
            get
            {
                if (eventRepository == null)
                {
                    eventRepository = new GenericRepository<Event>(context);
                }
                return eventRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
