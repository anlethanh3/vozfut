import { Alert, Col, Row, Table } from "react-bootstrap"
import { useAppDispatch, useAppSelector } from "../app/hooks"
import { fetchAsync, fetchWinnerTeam11Async, fetchWinnerTeam7Async, selectState } from "../slices/scoreboardSlice"
import { useEffect } from "react"

function Scoreboard() {

    const state = useAppSelector(selectState)
    const dispatch = useAppDispatch()

    const fetch = () => {
        dispatch(fetchAsync({})).unwrap()
            .catch(ex => { console.log(ex) })
    }

    useEffect(() => {
        console.log('effect', state)
        dispatch(fetchAsync({})).unwrap()
            .then(value => dispatch(fetchWinnerTeam7Async({})).unwrap())
            .then(value => dispatch(fetchWinnerTeam11Async({})).unwrap())
            .catch(ex => { console.log(ex) })

        return function cleanup() {
            console.log('unmount', state)
        }
    }, [])

    return (
        <>
            {
                state.isLoading &&
                <Alert color='primary'>Loading...</Alert>
            }
            <Row>
                <Col>
                    <h2>Leaderboard</h2>
                    <Table striped bordered hover>
                        <thead>
                            <tr>
                                <th>No</th>
                                <th>Member</th>
                                <th>Goal</th>
                                <th>Assist</th>
                            </tr>
                        </thead>
                        <tbody>
                            {
                                state.leaderboard && state.leaderboard.map((value, index) =>
                                    <tr key={`key-${index}`}>
                                        <td>{index + 1}</td>
                                        <td>{value.memberName}</td>
                                        <td>{value.goal}</td>
                                        <td>{value.assist}</td>
                                    </tr>
                                )
                            }
                        </tbody>
                    </Table>
                </Col>
                <Col>
                    <h2>Top Winner 11</h2>
                    <Table striped bordered hover>
                        <thead>
                            <tr>
                                <th>No</th>
                                <th>Member</th>
                                <th>Winner</th>
                            </tr>
                        </thead>
                        <tbody>
                            {
                                state.winnerTeam11 && state.winnerTeam11.map((value, index) =>
                                    <tr key={`key-${index}`}>
                                        <td>{index + 1}</td>
                                        <td>{value.memberName}</td>
                                        <td>{value.winner}</td>
                                    </tr>
                                )
                            }
                        </tbody>
                    </Table>
                </Col>
                <Col>
                    <h2>Top Winner 7</h2>
                    <Table striped bordered hover>
                        <thead>
                            <tr>
                                <th>No</th>
                                <th>Member</th>
                                <th>Winner</th>
                            </tr>
                        </thead>
                        <tbody>
                            {
                                state.winnerTeam7 && state.winnerTeam7.map((value, index) =>
                                    <tr key={`key-${index}`}>
                                        <td>{index + 1}</td>
                                        <td>{value.memberName}</td>
                                        <td>{value.winner}</td>
                                    </tr>
                                )
                            }
                        </tbody>
                    </Table>
                </Col>
            </Row>
        </>
    )
}
export default Scoreboard;