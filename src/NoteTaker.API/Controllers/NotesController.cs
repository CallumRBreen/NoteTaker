using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NoteTaker.API.Utilities;
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

            return notes.Select(n => new Note(n)).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Note> Get(string id)
        {
            return Ok(FakeDataHelper.GetNotes().FirstOrDefault());
        }

        [HttpPut("{id}")]
        public ActionResult<Note> Update(string id, UpdateNote note)
        {
            return Ok(FakeDataHelper.GetNotes().FirstOrDefault());
        }

        [HttpPost]
        public ActionResult<Note> Create(CreateNote note)
        {
            logger.LogDebug($"Created note");

            return Created($"api/notes/{Guid.NewGuid().ToString()}", new Note
            {
                Title = note.Title,
                Content = note.Content,
                Created = DateTime.UtcNow,
                Id = Guid.NewGuid().ToString(),
                Modified = DateTime.UtcNow
            });
        }

        [HttpPatch("{id}")]
        public ActionResult<Note> Patch(string id, JsonPatchDocument<Note> note)
        {
            if (note == null) return BadRequest();

            var patchedNote = new Note();

            note.ApplyTo(patchedNote);

            logger.LogDebug($"Patched note {patchedNote.Id}");

            return Ok(patchedNote);
        }
    }
}
