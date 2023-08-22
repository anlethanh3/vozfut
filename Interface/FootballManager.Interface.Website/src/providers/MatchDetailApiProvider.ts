import axios from "axios"
import { MatchDetailProps } from "../slices/matchDetailSlice"

const url = `${process.env.REACT_APP_API_URL}/matchdetail`

export const search = async <T>(props: { matchId: number, signal: AbortSignal }) => {
    let { signal, matchId } = props
    let response = await axios.post<T>(`${url}/search`,
        { matchId },
        { signal: signal, })
    return response
}

export const rolling = async <T>(props: { matchId: number, signal: AbortSignal }) => {
    let { signal, matchId } = props
    let response = await axios.get<T>(`${url}/rolling/${matchId}`,
        { signal: signal, })
    return response
}

export const remove = async (props: { signal: AbortSignal, id: number }) => {
    const { id, signal } = props
    let response = await axios.delete(`${url}/${id}`, { signal: signal, })
    return response
}

export const add = async (props: { signal: AbortSignal, data: MatchDetailProps }) => {
    let { signal, data } = props
    var response = await axios.post(`${url}`, data, { signal: signal, })
    return response
}

export const update = async (props: { signal: AbortSignal, data: MatchDetailProps }) => {
    let { signal, data } = props
    var response = await axios.put(`${url}`, data, { signal: signal, })
    return response
}
