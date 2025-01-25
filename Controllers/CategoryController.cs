using MediaSoft.Data;
using MediaSoft.Models;
using MediaSoft.Models.Entities;
using MediaSoft.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;


namespace MediaSoft.Controllers
{
    public class CategoryController : Controller
    {
        private readonly myDBContexts dBContexts;
        private readonly CryptoService _cryptoService;

        public CategoryController(myDBContexts dBContexts, CryptoService cryptoService)
        {
            this.dBContexts = dBContexts;
            this._cryptoService = cryptoService;
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new CategoryViewModel
            {
                NewCategory = new AddCategoryViewModel(),
                Categories = await dBContexts.Category.ToListAsync(),
                Notes = (await dBContexts.Note.ToListAsync()).Select(note => new NoteDB
                {
                    Id = note.Id,
                    CategoryId = note.CategoryId,
                    Content = _cryptoService.Decrypt(note.Content)
                }).ToList()
            };

            return View(model);
        }

        private bool IsBase64String(string base64String)
        {
            if (string.IsNullOrEmpty(base64String) || base64String.Length % 4 != 0
               || !Regex.IsMatch(base64String, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None))
            {
                return false;
            }
            try
            {
                var base64StringBytes = Convert.FromBase64String(base64String);
                return true;
            }
            catch (System.FormatException)
            {
                return false;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var category = new CategoryDB
                {
                    Name = viewModel.NewCategory.Name,
                };

                await dBContexts.Category.AddAsync(category);
                await dBContexts.SaveChangesAsync();

                viewModel.Categories = await dBContexts.Category.ToListAsync();
                viewModel.Notes = (await dBContexts.Note.ToListAsync()).Select(note => new NoteDB
                {
                    Id = note.Id,
                    CategoryId = note.CategoryId,
                    Content = _cryptoService.Decrypt(note.Content)
                }).ToList();
                viewModel.NewCategory = new AddCategoryViewModel();
            }
            else
            {
                viewModel.Categories = await dBContexts.Category.ToListAsync();
                viewModel.Notes = (await dBContexts.Note.ToListAsync()).Select(note => new NoteDB
                {
                    Id = note.Id,
                    CategoryId = note.CategoryId,
                    Content = IsBase64String(note.Content) ? _cryptoService.Decrypt(note.Content) : note.Content
                }).ToList();
            }
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var category = await dBContexts.Category.FindAsync(id);
            if (category != null)
            {
                dBContexts.Category.Remove(category);
                await dBContexts.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Add));
        }

        [HttpPost]
        public async Task<IActionResult> AddNote(Guid categoryId, string content)
        {
            var encryptedContent = _cryptoService.Encrypt(content);

            var note = new NoteDB
            {
                CategoryId = categoryId,
                Content = encryptedContent
            };

            await dBContexts.Note.AddAsync(note);
            await dBContexts.SaveChangesAsync();

            return RedirectToAction(nameof(Add));
        }

        [HttpPost]
        public async Task<IActionResult> EditNote(Guid id, string content)
        {
            var encryptedContent = _cryptoService.Encrypt(content);
            var note = await dBContexts.Note.FindAsync(id);

            if (note != null)
            {
                note.Content = encryptedContent;
                await dBContexts.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Add));
        }

        [HttpPost]
        public async Task<IActionResult> HidenNote(Guid id)
        {
            var model = new CategoryViewModel
            {
                NewCategory = new AddCategoryViewModel(),
                Categories = await dBContexts.Category.ToListAsync(),
                Notes = (await dBContexts.Note.ToListAsync()).Select(note => new NoteDB
                {
                        Id = note.Id,
                        CategoryId = note.CategoryId,
                        Content = _cryptoService.Decrypt(note.Content)
                    
                }).ToList(),
                EditingNoteId = id
            };

            return View("Add", model);
        }
    }
    }
