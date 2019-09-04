using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NoteTaker.API.ViewModels;
using NoteTaker.Core.Services.Interfaces;

 namespace NoteTaker.API.Controllers
{
    [Route("api/notes")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly ILogger<NotesController> logger;
        private readonly INotesService notesService;

        public NotesController(ILogger<NotesController> logger, INotesService notesService)
        {
            this.logger = logger;
            this.notesService = notesService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Note>>> Get([FromQuery] QueryNote query)
        {
            logger.LogDebug("Retrieving notes");

            var notes = await notesService.GetNotesAsync(query.Text);

            return Ok(notes.Select(n => new Note(n)).ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> Get(string id)
        {
            logger.LogDebug($"Retrieving note: {id}");

            var note = await notesService.GetNoteAsync(id);

            if (note == null)
            {
                logger.LogDebug($"Unable to find note: {id}");
                return NotFound();
            }

            return Ok(new Note(note));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Note>> Update(string id, [FromBody] UpdateNote note)
        {
            logger.LogDebug($"Updating note: {id}");

            var updatedNote = await notesService.UpdateNoteAsync(id, note.Title, note.Content);

            if (updatedNote == null)
            {
                logger.LogDebug($"Unable to find note: {id}");
                return NotFound();
            }

            logger.LogDebug($"Updated note: {id}");

            return Ok(new Note(updatedNote));
        }

        [HttpPost]
        public async Task<ActionResult<Note>> Create([FromBody]CreateNote note)
        {
            logger.LogDebug($"Creating note");

            var createdNote = await notesService.CreateNoteAsync(note.Title, note.Content);

            logger.LogDebug($"Created note: {createdNote.Id}");

            return Created($"api/notes/{createdNote.Id}", new Note(createdNote));
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Note>> Patch(string id, JsonPatchDocument<Note> note)
        {
            logger.LogDebug($"Patching note: {id}");

            var noteToPatch = await notesService.GetNoteAsync(id);

            if (noteToPatch == null)
            {
                logger.LogDebug($"Unable to find note: {id}");
                return NotFound();
            }

            var placeholderNote = new Note(noteToPatch);
            note.ApplyTo(placeholderNote);

            var patchedNote = await notesService.UpdateNoteAsync(id, placeholderNote.Title, placeholderNote.Content);

            logger.LogDebug($"Patched note {noteToPatch.Id}");

            return Ok(new Note(patchedNote));
        }
    }
}
