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

    if (error) return <p>{error}</p>
    if (!book) return <p>Loading...</p>

    return (
        <div>
            <Link to="/">← Back</Link>
            <Link to={`/books/${book.id}/edit`}>Edit</Link>
            <h1>{book.title}</h1>
            {book.titleSwedish && <p>Swedish title: {book.titleSwedish}</p>}
            <p>Year: {book.releaseYear}</p>
            <p>Genre: {book.genre.name}</p>
            <p>Short story: {book.isShortStory ? 'Yes' : 'No'}</p>
            {book.detectives.length > 0 && (
                <p>Detectives: {book.detectives.map(d => d.name).join(', ')}</p>
            )}
            {book.synopsis && <p>Synopsis: {book.synopsis}</p>}
            {book.trivia && <p>Trivia: {book.trivia}</p>}
        </div>
    )
}