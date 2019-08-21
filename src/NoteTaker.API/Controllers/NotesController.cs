using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NoteTaker.API.Utilities;
using NoteTaker.API.ViewModels;

namespace NoteTaker.API.Controllers
{
    [Route("api/notes")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly ILogger<NotesController> logger;

        public NotesController(ILogger<NotesController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<List<Note>> Get()
        {
            logger.LogDebug("Getting all notes");
            return Ok(FakeDataHelper.GetNotes(200));
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
