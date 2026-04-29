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

    if (error) return <p>{error}</p>

    return (
        <div>
            <h1>Books</h1>
            <ul>
                {books.map(book => (
                    <li key={book.id}>
                        <Link to={`/books/${book.id}`}>
                            {book.title} ({book.releaseYear})
                        </Link>
                        {' — '}
                        {book.genre.name}
                        {book.detectives.length > 0 && (
                            <span> — {book.detectives.map(d => d.name).join(', ')}</span>
                        )}
                    </li>
                ))}
            </ul>
        </div>
    )
}