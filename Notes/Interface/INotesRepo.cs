using Notes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Interface
{
    public interface INotesRepo
    {
        Task<IEnumerable<Note>> GetAll();
        Task<dynamic> AddNote(NotesParameter note);
        Task<dynamic> UpdateNote(NotesParameter note);
        Task<dynamic> DeleteNote(int id);

    }
}
