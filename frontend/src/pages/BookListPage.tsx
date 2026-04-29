import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { fetchBooks, fetchGenres, type Book, type Genre, type Detective } from '../api/books'
import './BookListPage.css'

export default function BookListPage() {
    const [books, setBooks] = useState<Book[]>([])
    const [error, setError] = useState<string | null>(null)
    const [genres, setGenres] = useState<Genre[]>([])
    const [selectedGenreId, setSelectedGenreId] = useState<string | null>(null)
    const [genreExpanded, setGenreExpanded] = useState(false)
    const [selectedDetectiveIds, setSelectedDetectiveIds] = useState<string[]>([])

    useEffect(() => {
        fetchBooks()
            .then(setBooks)
            .catch(() => setError('Could not load books'))
    }, [])

    useEffect(() => {
        fetchGenres().then(setGenres).catch(() => { })
    }, [])

    if (error) return <p className="error">{error}</p>

    const genreFilteredBooks = selectedGenreId
        ? books.filter(b => b.genre.id === selectedGenreId)
        : books

    const displayedBooks = selectedDetectiveIds.length > 0
        ? genreFilteredBooks.filter(b =>
            selectedDetectiveIds.every(id => b.detectives.some(d => d.id === id))
        )
        : genreFilteredBooks

    const availableDetectives: Detective[] = selectedGenreId
        ? Array.from(
            new Map(
                displayedBooks
                    .flatMap(b => b.detectives)
                    .filter(d => !selectedDetectiveIds.includes(d.id))
                    .map(d => [d.id, d])
            ).values()
        )
        : []

    function clearFilters() {
        setSelectedGenreId(null)
        setGenreExpanded(false)
        setSelectedDetectiveIds([])
    }

    return (
        <div className="book-list">
            <img src="/logo/Logo.svg" alt="Agatha Christie" className="book-list-logo" />
            <div className="filter-bar">
                <div className="filter-row">
                    {selectedGenreId && (
                        <button className="filter-clear" onClick={clearFilters}>
    <img src="/src/assets/icons/Icon_Cancel.svg" alt="Clear filters" />
</button>
                    )}
                    {!selectedGenreId ? (
                        <button
                            className={`filter-pill${genreExpanded ? ' filter-pill-active' : ''}`}
                            onClick={() => setGenreExpanded(e => !e)}
                        >
                            Genre
                        </button>
                    ) : (
                        <>
                            <span className="filter-pill filter-pill-l1">Genre</span>
                            <span className="filter-pill filter-pill-l2">
                                {genres.find(g => g.id === selectedGenreId)?.name}
                            </span>
                        </>
                    )}
                    {genreExpanded && !selectedGenreId && genres.map(g => (
                        <button
                            key={g.id}
                            className="filter-pill"
                            onClick={() => { setSelectedGenreId(g.id); setGenreExpanded(false); }} >
                            {g.name}
                        </button>
                    ))}
                    {selectedGenreId && selectedDetectiveIds.map(detId => {
                        const det = books.flatMap(b => b.detectives).find(d => d.id === detId)
                        return det ? (
                            <button
                                key={detId}
                                className="filter-pill filter-pill-l3"
                                onClick={() => setSelectedDetectiveIds(ids => ids.filter(id => id !== detId))} >
                                {det.name}
                            </button>
                        ) : null
                    })}
                    {selectedGenreId && availableDetectives.map(d => (
                        <button
                            key={d.id}
                            className="filter-pill"
                            onClick={() => setSelectedDetectiveIds(ids => [...ids, d.id])}
                        >
                            {d.name}
                        </button>
                    ))}
                </div>
            </div>
            <ul className="book-grid">
                {displayedBooks.map(book => (
                    <li
                        key={book.id}
                        className="book-card"
                        style={{ backgroundColor: book.detectives[0]?.hexColor ?? '#EBEBEB' }}
                    >
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