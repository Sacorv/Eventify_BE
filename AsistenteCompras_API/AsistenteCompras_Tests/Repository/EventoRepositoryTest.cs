using AsistenteCompras_API.Domain.Entities;
using AsistenteCompras_API.Infraestructure.Contexts;
using AsistenteCompras_API.Infraestructure.Repositories;

namespace AsistenteCompras_Tests.Repository;

public class EventoRepositoryTest
{
    public readonly AsistenteComprasContext _context;

	public EventoRepositoryTest()
	{
		_context = ContextMemory.Generate();
	}

	[Fact]
	public void QueSePuedaAgregarUnEvento()
	{
		Evento evento = new Evento();

		//evento.Id = 1;
		evento.Nombre = "Cumpleaños";
		evento.Estado = true;

		
		_context.Add(evento);

		_context.SaveChanges();


		Assert.Equal(1,evento.Id);
	}

	[Fact]
	public void ObtenerBebidasDelEvento()
	{
		var eventoRepo = new EventoRepository(_context);
		Evento evento = new Evento();
        Bebidum bebida = new Bebidum();
		Bebidum bebida2 = new Bebidum();

        evento.Nombre = "Cumpleaños";
		evento.Estado = true;



		bebida.TipoBebida = "bebida alcoholica";
		bebida2.TipoBebida = "bebida sin alcohol";


        _context.Add(evento);
		_context.Add(bebida);
		_context.Add(bebida2);
		_context.SaveChanges();

		EventoBebidum registroUno = new EventoBebidum();
        EventoBebidum registroDos = new EventoBebidum();

		registroUno.IdEvento = evento.Id;
		registroUno.IdBebida = bebida.Id;

		registroDos.IdEvento = evento.Id;
		registroDos.IdBebida = bebida2.Id;
		_context.Add(registroUno);
		_context.Add(registroDos);
		_context.SaveChanges();

		List<Bebidum> bebidasEvento = new List<Bebidum>();
		bebidasEvento = eventoRepo.ObtenerBebidas(evento.Id);


        Assert.Equal(2, bebidasEvento.Count);



        Assert.Equal(bebidasEvento.Last().TipoBebida,bebida.TipoBebida);
        Assert.Equal(bebidasEvento.First().TipoBebida,bebida2.TipoBebida);
    }

}
