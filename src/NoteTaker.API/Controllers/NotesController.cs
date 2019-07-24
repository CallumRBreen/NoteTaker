using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NoteTaker.API.Utilities;
using NoteTaker.API.ViewModels;

namespace NoteTaker.API.Controllers
{
    [Route("api/notes")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Note>> Get()
        {
            return Ok(FakeDataHelper.GetNotes());
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
            return Created($"api/notes/{Guid.NewGuid().ToString()}",FakeDataHelper.GetNotes().FirstOrDefault());
        }
    }
}
