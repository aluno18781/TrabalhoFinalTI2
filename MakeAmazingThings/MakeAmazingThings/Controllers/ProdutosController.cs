﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MakeAmazingThings.Models;

namespace MakeAmazingThings.Controllers
{
    public class ProdutosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Produtos
        // [ValidateAntiForgeryToken]
        public ActionResult Index(string btPesquisa)
        {
            // guarda a lista de produtos a serem apresentados, no ecrã, ao utilizador
            List<Produtos> listaProdutos = new List<Produtos>();

            if (!String.IsNullOrEmpty(btPesquisa) && !btPesquisa.Equals("Todos"))
            {
                // mostra apenas os produtos 'masculino', 'feminino' ou 'unisexo',
                // conforme a escolha do utilizador
                listaProdutos = db.Produtos.Where(p => p.SexoDoUtilizador.Equals(btPesquisa)).OrderByDescending(p => p.Ativo).ToList();
            }
            else
            {
                // não há filtragem de produtos
                listaProdutos = db.Produtos.OrderByDescending(p => p.Ativo).ToList();
            }

            //funcçao do where define se 
            return View(listaProdutos);
        }

        // GET: Produtos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produtos produtos = db.Produtos.Find(id);
            if (produtos == null)
            {
                return HttpNotFound();
            }
            return View(produtos);
        }

        // GET: Produtos/Create
        public ActionResult Create()
        {

            var listaDeOpcoes = new SelectList(new List<SelectListItem> {
                new SelectListItem { Text = "Unisexo", Value = "Unisexo"},
                new SelectListItem { Text = "Masculino", Value ="Masculino"},
                new SelectListItem { Text = "Feminino", Value = "Feminino"}
            }, "Text", "Value");

            ViewBag.SexoDoUtilizador = listaDeOpcoes;

            return View();
        }

        // POST: Produtos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NomeProd,Descricao,SexoDoUtilizador,Stock,Preco,IvaVenda,Ativo")] Produtos produto)
        {

            // avaliar se foi escolhido um tipo de público alvo
            // se não escolhi, devolver o objeto à view


            if (ModelState.IsValid)
            {
                db.Produtos.Add(produto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var listaDeOpcoes = new SelectList(new List<SelectListItem> {
                new SelectListItem { Text = "Unisexo", Value = "Unisexo"},
                new SelectListItem { Text = "Masculino", Value ="Masculino"},
                new SelectListItem { Text = "Feminino", Value = "Feminino"}
            }, "Text", "Value", produto.SexoDoUtilizador);

            ViewBag.SexoDoUtilizador = listaDeOpcoes;

            return View(produto);
        }

        // GET: Produtos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
             var listaDeOpcoes = new SelectList(new List<SelectListItem> {
                new SelectListItem { Text = "Unisexo", Value = "Unisexo"},
                new SelectListItem { Text = "Masculino", Value ="Masculino"},
                new SelectListItem { Text = "Feminino", Value = "Feminino"}
            }, "Text", "Value");

            ViewBag.SexoDoUtilizador = listaDeOpcoes;
            
            Produtos produtos = db.Produtos.Find(id);
            if (produtos == null)
            {
                return HttpNotFound();
            }
            return View(produtos);
        }

        // POST: Produtos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NomeProd,Descricao,Stock,Preco,IvaVenda,Ativo")] Produtos produtos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(produtos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var listaDeOpcoes = new SelectList(new List<SelectListItem> {
                new SelectListItem { Text = "Unisexo", Value = "Unisexo"},
                new SelectListItem { Text = "Masculino", Value ="Masculino"},
                new SelectListItem { Text = "Feminino", Value = "Feminino"}
            }, "Text", "Value", produtos.SexoDoUtilizador);

            ViewBag.SexoDoUtilizador = listaDeOpcoes;
            return View(produtos);
        }

        // GET: Produtos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produtos produtos = db.Produtos.Find(id);
            if (produtos == null)
            {
                return HttpNotFound();
            }
            return View(produtos);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Produtos produtos = db.Produtos.Find(id);
            db.Produtos.Remove(produtos);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
