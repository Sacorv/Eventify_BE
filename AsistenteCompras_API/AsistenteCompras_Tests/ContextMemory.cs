using AsistenteCompras_API.Infraestructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AsistenteCompras_Tests;

public static class ContextMemory
{
    public static AsistenteComprasContext Generate()
    {
        var optionBuilder = new DbContextOptionsBuilder<AsistenteComprasContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());

        return new AsistenteComprasContext(optionBuilder.Options);
    }
}
