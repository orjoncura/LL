using LL.CA.Configs;
using LL.Core.Enums;
using LL.Core.Interfaces.Services;
using LL.Core.Models.Arguments;
using LL.Core.Models.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LL.CA;

public class Program
{
    private static ICourseService courseService { get; set; }
    
    public static async Task Main(string[] args)
    {
        try
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
            
            // Add user secrets from Program.cs
            builder.Configuration.AddUserSecrets<Program>();
            
            builder.Services
                .AddDbContextConfig(builder.Configuration)
                .AddDependencyInjectionConfig(builder.Configuration);
            
            // Add logging
            builder.Services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Information);
            });
            
            courseService = builder.Services.BuildServiceProvider().GetRequiredService<ICourseService>();

            string text = "En una soleada mañana de sábado, María despertó con el sonido de las palomas graznando en el balcón. \"Buenos días\", murmuró mientras se estiraba y se sentaba en la cama. Hoy era un día especial: iba a \nvisitar al museo del arte moderno con sus amigos, algo que había estado deseando desde hace meses.\n\n\"¡María! ¿Estás lista?\" gritó su madre desde abajo. María se apresuró a vestirse con una camisa blanca y jeans claros, y se peinó rápidamente. \"Sí, mamá, ya bajo\", respondió, mientras colocaba sus zapatos \ndeportivos.\n\nBajando las escaleras, olió el aroma del café recién preparado. Su padre le sonrió y le ofreció una taza. \"Toma, te vendrá bien para el largo día que tienes\", dijo con amabilidad. María agradeció y dio un \nsorbo caliente, sintiendo cómo el calor se extendía por su cuerpo.\n\nLuego de desayunar, tomó su mochila ligera y salió de casa. La brisa suave acariciaba su rostro mientras caminaba hacia la estación de metro. Allí, subió al primer tren que llegó, agarrándose firmemente a \nlas barras metálicas para mantener el equilibrio.\n\nEl viaje fue corto, y pronto se encontró en la bulliciosa plaza central. Vio a sus amigos, Ana y Pedro, esperándola bajo un árbol. \"¡Hola! ¿Listos para pasar un gran día?\" exclamó María con entusiasmo.\n\nAna asintió mientras revisaba su reloj: \"Sí, pero antes de ir al museo, ¿te apetece comer algo? Tengo hambre\". Pedro rió y respondió: \"Claro, pero primero, ¿no deberíamos comprar boletos para el museo?\"\n\nMaría sacudió la cabeza. \"No, los compraremos allí. Pero antes, ¿qué tal tomamos un café en esa cafetería que vimos ayer?\" Ana y Pedro aceptaron, y juntos caminaron hacia una pequeña cafetería con mesas al \naire libre.\n\nMientras disfrutaban de sus capuchinos y croissants, charlaron sobre sus planes para el día. \"Ojalá podamos ver la exposición de arte contemporáneo\", comentó Pedro con interés. \"Siempre me gusta cómo los \nartistas modernos expresan sus ideas\".\n\nAna asintió en acuerdo. \"Sí, y también hay una sección dedicada a arte digital, algo que nunca he visto\". María sonrió, emocionada. \"También quiero ver las obras de mi artista favorito, Picasso. Aunque no \nes contemporáneo, su trabajo siempre me inspira\".\n\nDespués de terminar su café, caminaron hacia el museo. La entrada estaba rodeada de jardineras con flores coloridas que desprendían un aroma fresco. Algunos turistas sacaban fotos mientras otros esperaban \nen fila para comprar boletos.\n\n\"Vamos a comprar los nuestros\", dijo María decididamente. Mientras estaban en la cola, Pedro comentó: \"Ojalá el museo no esté muy concurrido; quiero disfrutar de las obras sin tanta multitud\". Ana rió y \nrespondió: \"Siempre hay curiosos que toman fotos con los cuadros\".\n\nFinalmente, llegaron a la taquilla. María compró tres boletos y les entregó uno a cada amigo. Al entrar al museo, se sintieron envueltos por el aroma de la pintura y el silencio reverencial del lugar.\n\nLaexpo sición de arte contemporáneo estaba en el segundo piso. Mientras caminaban por las galerías, sus ojos se deleitaban con colores vivos y formas abstractas. \"Este cuadro me hace sentir tranquila\", \nmurmuró Ana frente a una obra de tonos azules y verdes.\n\nPedro, por su parte, estaba fascinado con un videoarte proyectado en la pared. \"Es increíble cómo el artista utiliza tecnología para expresar sus ideas\", dijo maravillado.\n\nMaría se acercó a ellos y sonrió. \"Sí, es alucinante. Pero me encantaría ver las obras de Picasso también\". Decidieron subir al tercer piso, donde se encontraba la exposición dedicada al famoso artista.\n\nFrente al retrato de una mujer con ojos tristes, María suspiró. \"Siempre he pensado que Picasso podía capturar las emociones mejor que nadie\", comentó. Ana asintió y dijo: \"Es cierto, pero me pregunto qué \nle inspiraba a crear tales obras\".\n\nPedro señaló una serie de dibujos enmarcados en la pared. \"Miren estos bocetos, son tan detallados... Imagino que Picasso debía ser muy meticuloso con sus líneas\".\n\nMientras admiraban las obras, el sonido de las pisadas de los visitantes y el murmullo de las conversaciones llenaban el espacio. Algunas personas tomaban fotosfurtivas mientras otras se quedaban en \nsilencio, absorbiendo cada detalle.\n\nDe repente, escucharon una voz familiar: \"¡Hola chicos! ¿Qué tal laexpo sición?\" Se volvieron y vieron a su profesora de arte, señora López, sonriendo con amabilidad. \"Profesora... No la esperábamos aquí\", \ndijo María sorprendida.\n\n\"Me alegra ver que disfrutan el arte como yo\", respondió ella, acercándose para admirar un cuadro junto a ellos. \"Saben, Picasso no solo fue un genio en su época, sino que continúa inspirando a artistas de \ntodo el mundo\".\n\nAna asintió con entusiasmo. \"Es cierto. Y mira, hasta hay una expo sición especial sobre sus obras\". Pedro se acercó al mostrador y preguntó: \"¿Dónde podemos encontrarla?\"\n\nLa señora López sonrió y respondió: \"En el piso superior. Es unaexpo sición muy interesante que muestra cómo su estilo evolucionó a través de los años\".\n\nDecidieron subir al piso superior para explorar laexpo sición especial sobre Picasso. Allí, vieron desde sus trabajos más tempranos hasta las etapas más innovadoras de su carrera.\n\n\"Es increíble cómo cambió su estilo\", comentó Ana frente a una serie cronológica de cuadros. \"Desde el realismo hasta el cubismo, parece que siempre estuvo buscando nuevas formas de expresión\".\n\nMaría asintió en silencio, sintiendo una profunda admiración por el artista. Pedro, por su parte, tomó algunas fotos con su teléfono para recordar los detalles.\n\nDespués de pasar horas en el museo, salieron cansados pero satisfechos. El sol comenzaba a declinar, y las sombras se alargaban sobre la plaza.\n\n\"Gracias por venir conmigo\", dijo María mientras caminaban hacia la estación de metro. \"Hoy ha sido un día maravilloso\". Ana y Pedro sonrieron en respuesta. \"Fue divertido compartir esto contigo\", dijo Ana, \ny Pedro añadió: \"Definitivamente, el arte nos une de manera especial\".\n\nMientras se despedían en la estación, María sintió una mezcla de satisfacción y nostalgia. Sabía que aquel día quedaría grabado en su memoria como uno de los mejores. Con un suspiro de contento, pensó: \"El \narte realmente te hace ver el mundo con otros ojos\".\n\nMaría caminaba hacia su casa, sintiendo cómo la brisa nocturna acariciaba su rostro. Aquel día había sido especial, no solo por laexpo sición de arte, sino también porque había compartido esos momentos tan \nsignificativos con sus amigos.\n\nMientras las luces de la ciudad brillaban a su alrededor, pensó en cómo el arte les había permitido conectarse de manera profunda y auténtica. \"Gracias al arte\", murmuró para sí misma, \"he encontrado un \nespacio donde mis sueños y realidad se mezclan\".\n\nAl llegar a su casa, encendió una vela aromática y se sentó frente a su cuaderno de dibujo. Con lápiz en mano, comenzó a esbozar sus emociones del día, sintiendo cómo cada trazo reflejaba la inspiración que \nhabían compartido.\n\nEra noche cerrada cuando finalmente se acostó, con una sonrisa satisfecha. Sabía que mañana vendría con nuevos retos y sueños, pero aquella noche, bajo el manto estrellado del cielo, se sintió agradecida \npor las conexiones que había construido y las experiencias que habían enriquecido su vida.\n\nMaría caminaba hacia su casa, sintiendo cómo la brisa nocturna acariciaba su rostro. Aquel día había sido especial, no solo por laexpo sición de arte, sino también porque había compartido esos momentos tan \nsignificativos con sus amigos.\n\nMientras las luces de la ciudad brillaban a su alrededor, pensó en cómo el arte les había permitido conectarse de manera profunda y auténtica. \"Gracias al arte\", murmuró para sí misma, \"he encontrado un \nespacio donde mis sueños y realidad se mezclan\".\n\nAl llegar a su casa, encendió una vela aromática y se sentó frente a su cuaderno de dibujo. Con lápiz en mano, comenzó a esbozar sus emociones del día, sintiendo cómo cada trazo reflejaba la inspiración que \nhabían compartido.\n\nEra noche cerrada cuando finalmente se acostó, con una sonrisa satisfecha. Sabía que mañana vendría con nuevos retos y sueños, pero aquella noche, bajo el manto estrellado del cielo, se sintió agradecida \npor las conexiones que había construido y las experiencias que habían enriquecido su vida.\n\n";
            //string text = "En una soleada mañana de sábado, María despertó con el sonido de las palomas graznando en el balcón. \"Buenos días\", murmuró mientras se estiraba y se sentaba en la cama. Hoy era un día especial: iba a \nvisitar al museo del arte moderno con sus amigos, algo que había estado deseando desde hace meses.\n\n\"¡María! ¿Estás lista?\" gritó su madre desde abajo. María se apresuró a vestirse con una camisa blanca y jeans claros, y se peinó rápidamente. \"Sí, mamá, ya bajo\", respondió, mientras colocaba sus zapatos \ndeportivos.\n\nBajando las escaleras, olió el aroma del café recién preparado. Su padre le sonrió y le ofreció una taza. \"Toma, te vendrá bien para el largo día que tienes\", dijo con amabilidad. María agradeció y dio un \nsorbo caliente, sintiendo cómo el calor se extendía por su cuerpo.\n\nLuego de desayunar, tomó su mochila ligera y salió de casa. La brisa suave acariciaba su rostro mientras caminaba hacia la estación de metro. Allí, subió al primer tren que llegó, agarrándose firmemente a \nlas barras metálicas para mantener el equilibrio.\n\nEl viaje fue corto, y pronto se encontró en la bulliciosa plaza central. Vio a sus amigos, Ana y Pedro, esperándola bajo un árbol. \"¡Hola! ¿Listos para pasar un gran día?\" exclamó María con entusiasmo.";
            
            CourseRequestModel courseRequestModel = new()
            {
                Text = text.Replace("/[\r\n]+/g", " "),
                LanguageFromId = (int)LanguageEnum.Spanish,
                LanguageToId = (int)LanguageEnum.English
            };

            CourseViewModel courseViewModel = await courseService.CreateCourse(courseRequestModel, 1);
            
            foreach (var courseWordsModel in courseViewModel.Words)
            {
                DefinitionRequestModel definitionRequestModel = new()
                {
                    Text = courseWordsModel.Word,
                    Translation = courseWordsModel.Translation,
                    LanguageFromId = courseRequestModel.LanguageFromId,
                    LanguageToId = courseRequestModel.LanguageToId
                };
            
                await courseService.CreateDefinitions(definitionRequestModel, 1);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
