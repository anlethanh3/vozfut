import axios from "axios"

const url = `${process.env.REACT_APP_API_URL}/scoreboard`

export const getLeaderboard = async <T>(props: { signal: AbortSignal }) => {
    let { signal } = props
    let response = await axios.get<T>(`${url}/leaderboard`, { signal: signal })
    return response
}

export const getWinner = async <T>(props: { signal: AbortSignal, teamSize: number }) => {
    let { teamSize, signal } = props
    let response = await axios.get<T>(`${url}/winner?teamSize=${teamSize}`, { signal: signal })
    return response
}
