import { useEffect, useState } from 'react'
import { useParams, Link } from 'react-router-dom'
import { fetchBook, type Book } from '../api/books'
import './BookDetailPage.css'

export default function BookDetailPage() {
    const { id } = useParams<{ id: string }>()
    const [book, setBook] = useState<Book | null>(null)
    const [error, setError] = useState<string | null>(null)

    useEffect(() => {
        if (!id) return
        fetchBook(id)
            .then(setBook)
            .catch(() => setError('Could not load book'))
    }, [id])

    if (error) return <p className="error">{error}</p>
    if (!book) return <p>Loading...</p>

    const heroColor = book.detectives[0]?.hexColor ?? '#EBEBEB'

    return (
        <div className="book-detail">
            <div className="book-detail-hero" style={{ backgroundColor: heroColor }}>
                <div className="book-detail-hero-nav">
                    <Link to="/"><img src="/src/assets/icons/Icon_Arrow_Left_Bold.svg" alt="Back" /></Link>
                    <Link to={`/books/${book.id}/edit`}><img src="/src/assets/icons/Icon_Edit.svg" alt="Edit" /></Link>
                </div>
                <h1 className="book-detail-title">{book.title}</h1>
                {book.titleSwedish && <p className="book-detail-title-swedish">{book.titleSwedish}</p>}
            </div>
            <div className="book-detail-body">
                <p className="book-detail-meta">
                    {book.releaseYear} | {book.genre.name}
                    {book.detectives.length > 0 && ` | ${book.detectives.map(d => d.name).join(', ')}`}
                </p>
                {book.synopsis && (
                    <section>
                        <h2>Synopsis</h2>
                        <p>{book.synopsis}</p>
                    </section>
                )}
                {book.trivia && (
                    <section>
                        <h2>Trivia</h2>
                        <p>{book.trivia}</p>
                    </section>
                )}
            </div>
        </div>
    )
}
