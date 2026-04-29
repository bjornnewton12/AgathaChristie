import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { fetchBooks, type Book } from '../api/books'
import './BookListPage.css'

export default function BookListPage() {
    const [books, setBooks] = useState<Book[]>([])
    const [error, setError] = useState<string | null>(null)

    useEffect(() => {
        fetchBooks()
            .then(setBooks)
            .catch(() => setError('Could not load books'))
    }, [])

    if (error) return <p className="error">{error}</p>

    return (
        <div className="book-list">
            <img src="/logo/Logo.svg" alt="Agatha Christie" className="book-list-logo" />
            <ul className="book-grid">
                {books.map(book => (
                    <li
                        key={book.id}
                        className="book-card"
                        style={{ backgroundColor: book.detectives[0]?.hexColor ?? '#EBEBEB' }}>
                        <Link to={`/books/${book.id}`} className="book-card-link">
                            <span className="book-card-title">{book.title}</span>
                            <span className="book-card-meta">
                                {book.releaseYear} | {book.genre.name}
                                {book.detectives.length > 0 && ` | ${book.detectives.map(d => d.name).join(', ')}`}
                            </span>
                        </Link>
                    </li>
                ))}
            </ul>
        </div>
    )
}